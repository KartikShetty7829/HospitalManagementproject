namespace Auth_Service.Interfaces
{
    public interface IUnitOfWork <Tcontext>
    {
       
        Task SaveChangesAsync();
    }
}
