namespace Nemetschek.API.Contract.Constants
{
    /// <summary>
    /// API routes constants
    /// </summary>
    public static class ApiRoutes
    {
        //Base URL for the API
        public const string Base = "api";

        public const string Ping = Base + "/ping";

        //Create user api endpoint
        public const string Users = Base + "/users";

        //Auth api endpoint
        public const string Auth = Base + "/auth";

        //Base Dice API endpoint url
        public const string Dice = Base + "/dice";

        //Roll dice endpoint
        public const string Roll = Dice + "/roll";

        //Get history endpoint
        public const string History = Dice + "/history";
    }
}
