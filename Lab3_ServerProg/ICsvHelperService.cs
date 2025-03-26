namespace Lab3_ServerProg
{
    public interface ICsvHelperService
    {
        Task SaveRecordAsync(ContactRecord record);
    }
}
