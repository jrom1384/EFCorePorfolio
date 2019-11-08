namespace EFCore.Common
{
    public class ErrorMessages
    {
        public const string InvalidParameterValues = "Entries feed on the operation was canceled due to invalid values. Please provide new entries.";

        public const string RecordAlreadyExists = "already exists.";

        public const string JsonPatchException = "Unable to parse Json Patch Document, the operation was canceled.";

        public const string DBUpdateConcurrency = "The record you attempted to edit was modified by another user after you got the original value." +
            "The edit operation was canceled, If you still want to edit this record, click the Save button again. Otherwise Back to List hyperlink.";

        public const string DBDeleteConcurrency = "The record you attempted to delete was modified by another user after you got the original value." +
            "The delete operation was canceled, If you still want to delete this record, click the Delete button again. Otherwise Back to List hyperlink.";

        public const string DBDeleteRecordWasInUse = "The record you attempted to delete was in use. The delete operation was canceled.";

        public const string DBUpdateException = "Database exception, the operation was canceled.";

        public const string UnhandledException = "Unhandled exception please contact support team thanks.";
    }
}
