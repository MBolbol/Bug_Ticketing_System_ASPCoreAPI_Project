namespace BugTicketingSystem.BL.Constants
{
    public static class ErrorConstants
    {
        public static class ErrorMessages
        {
            public const string NameRequired = "Name Can't be empty";
            public const string EmailRequired = "Email Can't be empty";
            public const string FilePathRequired = "File Path Can't be empty";
            public const string UserRoleRequired = "User Role Required";
            public const string BugStatusRequired = "Bug Status Required";
            public const string NameCannotBeShorterThan3Characters = "Name cannot be shorter than 3 characters";
            public const string NameMustBeUnique = "Name must be unique";
            public const string EmailMustBeUnique = "Email must be unique";
            public const string FileNameMustBeUnique = "File Name must be unique";
            public const string ProjectNameMustBeUnique = "project Name must be unique";
        }
        public static class ErrorCodes
        {
            public const string NameRequired = "ERR-01";
            public const string EmailRequired = "ERR-09";
            public const string FilePathRequired = "ERR-10";
            public const string UserRoleRequired = "ERR-02";
            public const string BugStatusRequired = "ERR-03";
            public const string NameMustBeUnique = "ERR-04";
            public const string NameCannotBeShorterThan3Characters = "ERR-05";
            public const string EmailMustBeUnique = "ERR-06";
            public const string FileNameMustBeUnique = "ERR-07";
            public const string ProjectNameMustBeUnique = "ERR-08";

        }
    }

}
