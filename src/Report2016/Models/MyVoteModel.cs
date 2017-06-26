using System;
namespace Report2016.Models
{
    public class MyVoteModel
    {
        public string Yes { get; set; }
        public string No { get; set; }
        public string NotSure { get; set; }

        public string Comment { get; set; }



        public bool NotVoted(){
            return Yes == null && No == null && NotSure == null;
        }
    }
}
