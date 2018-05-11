using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTFMessenger
{
    public class RTFMessenger
    {
        public Boolean UsesRTF;
        public Boolean LightBackground;
        public RichTextBox RTB;
        public TextBox TB;
        public RTFMessage CurrentMessage;
        String CurrentMessageType;

        public Int32 DefaulSpaceAfter = 120, DefaultSpaceBefore = 0;

        public RTFMessenger(RichTextBox rtb, Boolean lightBackground = false)
        {
            UsesRTF = true;
            RTB = rtb;
            LightBackground = lightBackground;
        }

        public RTFMessenger(TextBox tb, Boolean lightBackground = false)
        {
            UsesRTF = false;
            TB = tb;
            LightBackground = lightBackground;
        }

        public RTFMessage NewMessage(String MessageType="Info")
        {
            CurrentMessageType = "Info";   
            CurrentMessage = new RTFMessage(this);
            return CurrentMessage;
        }        

        void LogMessageToRTF(RTFMessage MessageToLog)
        {
            RTB.SelectedRtf = MessageToLog.AsRTF();
            RTB.SelectionStart = RTB.Text.Length;
            RTB.ScrollToCaret();
        }

        void LogMessageToPlainText(Message MessageToLog)
        {
            TB.AppendText(MessageToLog.ToString());
        }  
        
        public void Log()
        {
            CurrentMessage.Log();
        }
    }

    public class RTFMessage
    {
        RTFMessenger MessengerData;
        List<RTFMessageElement> MessageTextElements = new List<RTFMessageElement>();
        String _Type =  "Info";
        Boolean OverrideSpaceAfter = false, OverrideSpaceBefore = false;
        Int32 _SpaceAfter, _SpaceBefore;

        public RTFMessage(RTFMessenger messengerData)
        {
            MessengerData = messengerData;
        }

        private String Type
        {
            get{ return _Type; }
        }        

        public Boolean UsesLightBackground
        {
            get { return MessengerData.LightBackground; }
        }

        public String RTFTextColorByMessageType
        {
            get
            {
                switch (Type)
                {
                    case "Warning":
                        return @"\cf2";
                    case "Error":
                        return @"\cf3";
                    default:
                        return @"\cf1";
                }
            }
        }

        private String HeaderDarkBackground = @"{\rtf1\ansi\fs20{\colortbl;\red189\green174\blue137;\red255\green118\blue0;\red255\green0\blue0;}";
        private String HeaderLightBackground = @"{\rtf1\ansi\fs20{\colortbl;\red0\green0\blue0;\red255\green118\blue0;\red255\green0\blue0;}";

        public String MyHeader
        {
            get { return UsesLightBackground ? HeaderLightBackground : HeaderDarkBackground; }
        }

        public String FormatAsRTF(String TargetRTFText)
        {
            return String.Format(@"{0}{1}}}", MyHeader, TargetRTFText);
        }

        public RTFMessage StartBold()
        {
            MessageTextElements.Add(new RTFMessageElement(@"\b ", true));
            return this;
        }

        public RTFMessage EndBold()
        {
            MessageTextElements.Add(new RTFMessageElement(@"\b0 ", true));
            return this;
        }

        public RTFMessage AddText(String MessageText)
        {
            MessageTextElements.Add(new RTFMessageElement(MessageText, false));
            return this;
        }

        public RTFMessage SetSpaceAfter(Int32 spaceAfter)
        {
            OverrideSpaceAfter = true;
            _SpaceAfter = spaceAfter;
            return this;
        }

        public String SpaceAfter
        {
            get { return String.Format(@"\sa{0} ", OverrideSpaceAfter ? _SpaceAfter : MessengerData.DefaulSpaceAfter); }
        }

        public RTFMessage SetSpaceBefore(Int32 spaceBefore)
        {
            OverrideSpaceBefore = true;
            _SpaceBefore = spaceBefore;
            return this;
        }
        public String SpaceBefore
        {
            get { return String.Format(@"\sb{0} ", OverrideSpaceBefore ? _SpaceBefore : MessengerData.DefaultSpaceBefore); }
        }

        public RTFMessage NewLine()
        {
            MessageTextElements.Add(new RTFMessageElement(@"\line ", true));
            return this;
        }

        public RTFMessage IndentFirstLine()
        {
            MessageTextElements.Insert(0, new RTFMessageElement(@"\fi400 ", true));
            return this;
        }

        public RTFMessage IndentHanging()
        {     
            MessageTextElements.Insert(0, new RTFMessageElement(@"\fi-400 \li400 ", true));
            return this;
        }

        public RTFMessage AddBoldText(String MessageText)
        {
            RTFMessage Temp = new RTFMessage(MessengerData).StartBold().AddText(MessageText).EndBold();
            MessageTextElements.AddRange(Temp.MessageTextElements);
            return this;
        }

        public RTFMessage IsError()
        {
            _Type = "Error";
            return this;
        }

        public RTFMessage IsWarning()
        {
            _Type = "Warning";
            return this;
        }

        public RTFMessage PrependMessageType()
        {
            RTFMessage Temp = new RTFMessage(MessengerData).StartBold().AddText(Type).AddText(": ").EndBold();
            MessageTextElements.InsertRange(0, Temp.MessageTextElements);
            return this;
        }

        public void Log()
        {
            if (UsesRTF) { LogAsRTF(); } else { LogAsPlainText(); }
        }

        private Boolean UsesRTF
        {
            get { return MessengerData.UsesRTF; }
        }

        public String AsRTF()
        {
            String NewRTF;
            String Elements = String.Join(String.Empty, MessageTextElements.Select(o=> o.Text).ToList());
            NewRTF = String.Format(@"\pard{0}{1}{2}{3}\par ", RTFTextColorByMessageType, SpaceBefore, SpaceAfter, Elements);//add space before and space after
            return FormatAsRTF(NewRTF);
        }

        public String AsPlainText()
        {
            return String.Join(String.Empty, MessageTextElements.Where(o=> !o.IsRTFTag).Select(o=>o.Text) );
        }

        public void LogAsRTF()
        {
            MessengerData.RTB.SelectedRtf = AsRTF();
        }

        public void LogAsPlainText()
        {
            MessengerData.TB.AppendText(String.Format("{0}{1}", AsPlainText(), Environment.NewLine));
        }       

        private class RTFMessageElement
        {
            public String Text;
            public Boolean IsRTFTag;
            public RTFMessageElement(String text, Boolean isRTFTag)
            {
                Text = text; IsRTFTag = isRTFTag;
            }
        }
    }
}
