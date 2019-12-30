using AutoMapper;
using LeadChina.ProjectManager.SysSetting.Entity;
using LeadChina.ProjectManager.SysSetting.Entity.ViewModel;
using NanoFabric.Core;
using NanoFabric.Core.Helper;
using NanoFabric.Infrastrue.Mycat.Repository;

namespace LeadChina.ProjectManager.SysSetting.BusiProcess.Impl
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
            return _mapper.Map<AccountViewModel>(_accountRepository.FindOneAsNoTracking(
                _ => _.AccountNo == username && _.Password == MD5Helper.MD5Encrypt32(password)));
        }
    }
}
