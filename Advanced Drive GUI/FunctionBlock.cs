namespace Advanced_Drive_GUI
{
    public class FunctionBlock : IComparable
    {
        internal int highestContainedId = 0;
        internal TabPage? tabPage;

        public string name { get; set; } = "";
        public string? description { get; set; }

        public List<Parameter> parameters { get; set; } = new List<Parameter>();

        /// <summary>
        /// This function should help sort the functionblocks by their highest contained ID
        /// </summary>
        /// <param name="incomingobject">Another FunctionBlock usually</param>
        /// <returns></returns>
        public int CompareTo(object? incomingobject)
        {

            if (incomingobject is null) //If incomingobject is null
            {
                return 0; //Return zero
            } 
            else if (incomingobject is FunctionBlock functionBlock) //If it's a functionblock as it should be
            {
                return highestContainedId.CompareTo(functionBlock.highestContainedId); //Use the their highest contained ID to compare
            } 
            else //If it's something else
            {
                return 0; //Don't compare
            }

        }
    }
}