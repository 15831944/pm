using AutoMapper;
using LeadChina.PM.Core.Helper;
using LeadChina.PM.SysSetting.Domain;
using LeadChina.PM.SysSetting.Domain.ViewModel;
using LeadChina.PM.SysSetting.Infrastrue.Repository;

namespace LeadChina.PM.SysSetting.Application
{
    public class AccountService : BaseService<AccountViewModel, Account, int>, IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;

        protected override IRepository<Account, int> Repository { get => _accountRepository; }

        public AccountService(IMapper mapper, IUnitOfWork unitOfWork
            , IAccountRepository accountRepository) : base(mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// 根据工号，密码获取用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AccountViewModel GetAcount(string username, string password)
        {
            string pwd = MD5Helper.MD5Encrypt32(password);
            var user = _accountRepository.FindOneAsNoTracking(
                _ => _.AccountNo == username && _.Password == pwd);
            return _mapper.Map<AccountViewModel>(user);
        }
    }
}
