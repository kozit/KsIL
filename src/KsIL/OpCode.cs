namespace KsIL
{
    public enum OpCode
    {

        Unused = 0x00,

        Null = 0x01,

        Interrupt = 0x02,
        Interrupt_CallBack = 0x04,

        //Tests
        Test_Equal = 0x11,
        Test_Greater_Than = 0x12,

        //Math
        Add = 0x21, 
        Subtract = 0x22,
        Divide = 0x23,
        Mutiply = 0x24,
        MathSpecial = 0x25,

        //Memory
        Compare = 0x31,
        Move = 0x32,
        Clear = 0x33,
        Clear_Pointer = 0x34,

        Store = 0x35,
        Store_Pointer = 0x36,

        //CPU info, runtime info
        MemorySpecial = 0x37,
        
        Goto = 0x51,
        //Label = 0x52,
        Call = 0x53,
        Return = 0x54,

        DubbleOP = 0xFF

    }
}
