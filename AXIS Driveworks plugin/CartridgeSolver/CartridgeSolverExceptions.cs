using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartridgePickerExceptions
{
    public class RequestedLengthTooShort : Exception
    {
        public RequestedLengthTooShort()
            : base(message: "CartridgePicker: The requested length is shorter than the shortest cartridge.")
        {
        }

        //public FractionNotProperException(string message)
        //    : base(message)
        //{
        //}

        //public FractionNotProperException(string message, Exception inner)
        //    : base(message, inner)
        //{
        //}
    }

    public class RequestedLengthTooLong : Exception
    {
        public RequestedLengthTooLong()
            : base(message: "CartridgePicker: This solver deals with cartridge setups within a single extrusion. The requested length is longer than our max length per section of 149.875\"")
        {
        }

        //public FractionNotProperException(string message)
        //    : base(message)
        //{
        //}

        //public FractionNotProperException(string message, Exception inner)
        //    : base(message, inner)
        //{
        //}
    }
}
