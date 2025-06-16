namespace ControleDeBar.Dominio.AccountModule;

public interface IAccountRepository
{
    void RegisterAccount(Account account);
    Account SelectById(Guid id);
    List<Account> SelectAccounts();
    List<Account> SelectOpenAccounts();
    List<Account> SelectClosedAccounts();
    List<Account> SelectAccountsByDate(DateTime date);
}
