using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineShop.Models;
using OnlineShop.Model.Models;
using OnlineShop.Data.Repositories;
using OnlineShop.Data.Infrastructure;
using OnlineShop.Service.Services.Token;

namespace OnlineShop.AppStart
{
    public class IdentityUserInitializer : Disposable
    {
        private readonly ILogger<IdentityUserInitializer> _logger;
        private IApplicationGroupRepository? _groupRepository;
        private IApplicationRoleRepository? _roleRepository;
        private IApplicationRoleGroupRepository? _roleGroupRepository;
        private IApplicationUserGroupRepository? _userGroupRepository;

        private IUnitOfWork? _unitOfWork;
        private readonly UserManager<ApplicationUser>? _userManager;
        private readonly RoleManager<ApplicationRole>? _roleManager;
        private IHostApplicationLifetime _lifeTime;
        private IConfiguration _configuration;

        public static readonly string managerUsername = "first_admin";
        public static readonly string managerEmail = "first_admin@test";
        public static readonly string managerPassword = "!Init01";      
        public static readonly string workerUsername = "first_worker";
        public static readonly string workerEmail = "first_worker@test";
        public static readonly string workerPassword = "!Init01";              

        public IdentityUserInitializer(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILogger<IdentityUserInitializer>>()!;
            _unitOfWork = serviceProvider.GetService<IUnitOfWork>();
            _groupRepository = serviceProvider.GetService<IApplicationGroupRepository>();
            _roleRepository = serviceProvider.GetService<IApplicationRoleRepository>();
            _roleGroupRepository = serviceProvider.GetService<IApplicationRoleGroupRepository>();
            _userGroupRepository = serviceProvider.GetService<IApplicationUserGroupRepository>();
            _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            _roleManager = serviceProvider.GetService<RoleManager<ApplicationRole>>();
            _lifeTime = serviceProvider.GetService<IHostApplicationLifetime>()!;
            _configuration = serviceProvider.GetService<IConfiguration>()!;

        }

        public async Task Initialize()
        {
            if (!AddGroups())
            {
                _lifeTime.StopApplication();
            }
            _unitOfWork!.Commit();
            if (!await AddRoles())
            {
                _lifeTime.StopApplication();
            }
            _unitOfWork!.Commit();
            if (!await AddRoleGroups())
            {
                _lifeTime.StopApplication();
            }
            _unitOfWork!.Commit();
            
            if (!await AddMaintenanceUsers())
            {
                _lifeTime.StopApplication();
            }
            _unitOfWork!.Commit();

            if (!await AddUserGroups())
            {
                _lifeTime.StopApplication();
            }
            _unitOfWork!.Commit();

            if (!await AddUserRoles())
            {
                _lifeTime.StopApplication();
            }
            _unitOfWork!.Commit();
        }

        private bool AddGroups()
        {
            try
            {
                var maintenanceGr = _groupRepository!.GetSingleByCondition(x => x.Name == Group.Maintenancer);
                if (maintenanceGr == null)
                {
                    _groupRepository.Add(new ApplicationGroup
                    {
                        Name = Group.Maintenancer,
                        Description = "Group danh cho quan tri vien"
                    });
                }
                var customerGr = _groupRepository!.GetSingleByCondition(x => x.Name == Group.Customer);
                if(customerGr == null)
                {
                    _groupRepository.Add(new ApplicationGroup
                    {
                        Name = Group.Customer,
                        Description = "Group danh cho khach hang"
                    });
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Occurred error when AddGroups: ", ex.ToString());
                return false;
            }
            
            return true;
        }

        private async Task<bool> AddRoles()
        {
            try
            {
                var adminRole = await _roleManager!.FindByNameAsync(Role.Admin);
                if (adminRole == null)
                {
                    var ret = await _roleManager!.CreateAsync(new ApplicationRole
                    {
                        Name = Role.Admin,
                        Description = "Role admin danh cho quan tri vien"
                    });
                    if (ret != null && ret.Succeeded == false)
                    {
                        _logger.LogError(ret.Errors.ToList().ToString());
                        return false;
                    }
                }

                var workerRole = await _roleManager!.FindByNameAsync(Role.Worker);
                if (workerRole == null)
                {
                    var ret = await _roleManager!.CreateAsync(new ApplicationRole
                    {
                        Name = Role.Worker,
                        Description = "Role worker danh cho quan tri vien"
                    });
                    if (ret != null && ret.Succeeded == false)
                    {
                        _logger.LogError(ret.Errors.ToList().ToString());
                        return false;
                    }
                }

                var vipmemberRole = await _roleManager!.FindByNameAsync(Role.VipMember);
                if (vipmemberRole == null)
                {
                    var ret = await _roleManager!.CreateAsync(new ApplicationRole
                    {
                        Name = Role.VipMember,
                        Description = "Role vip member danh cho quan khach hang"
                    });
                    if (ret != null && ret.Succeeded == false)
                    {
                        _logger.LogError(ret.Errors.ToList().ToString());
                        return false;
                    }
                }

                var basicmemberRole = await _roleManager!.FindByNameAsync(Role.BasicMember);
                if (basicmemberRole == null)
                {
                    var ret = await _roleManager!.CreateAsync(new ApplicationRole
                    {
                        Name = Role.BasicMember,
                        Description = "Role basic member danh cho quan khach hang"
                    });
                    if (ret != null && ret.Succeeded == false)
                    {
                        _logger.LogError(ret.Errors.ToList().ToString());
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Occurred error when AddRoles: ", ex.ToString());
                return false;
            }
            return true;
        }

        private async Task<bool> AddRoleGroups()
        {
            try
            {
                // maintenancer
                var maintenanceGr = _groupRepository!.GetSingleByCondition(x => x.Name == Group.Maintenancer);             
                var adminRole = await _roleManager!.FindByNameAsync(Role.Admin);
                var workerRole = await _roleManager!.FindByNameAsync(Role.Worker);
                if(maintenanceGr != null && adminRole != null)
                {
                    var roleGroup = _roleGroupRepository!.GetSingleByCondition(x => x.GroupId == maintenanceGr.ID && x.RoleId == adminRole.Id);
                    if(roleGroup == null)
                    {
                        _roleGroupRepository!.Add(new ApplicationRoleGroup
                        {
                            RoleId = adminRole.Id.ToString(),
                            GroupId = maintenanceGr.ID
                        });
                    }               
                }
                if (maintenanceGr != null && workerRole != null)
                {
                    var roleGroup = _roleGroupRepository!.GetSingleByCondition(x => x.GroupId == maintenanceGr.ID && x.RoleId == workerRole.Id);
                    if (roleGroup == null)
                    {
                        _roleGroupRepository!.Add(new ApplicationRoleGroup
                        {
                            RoleId = workerRole.Id.ToString(),
                            GroupId = maintenanceGr.ID
                        });
                    }             
                }

                // customer
                var customerGr = _groupRepository!.GetSingleByCondition(x => x.Name == Group.Customer);
                var vipmemberRole = await _roleManager!.FindByNameAsync(Role.VipMember);
                var basicmemberRole = await _roleManager!.FindByNameAsync(Role.BasicMember);
                if (customerGr != null && vipmemberRole != null)
                {
                    var roleGroup = _roleGroupRepository!.GetSingleByCondition(x => x.GroupId == customerGr.ID && x.RoleId == vipmemberRole.Id);
                    if(roleGroup == null)
                    {
                        _roleGroupRepository!.Add(new ApplicationRoleGroup
                        {
                            RoleId = vipmemberRole.Id.ToString(),
                            GroupId = customerGr.ID
                        });
                    }                
                }
                if (customerGr != null && basicmemberRole != null)
                {
                    var roleGroup = _roleGroupRepository!.GetSingleByCondition(x => x.GroupId == customerGr.ID && x.RoleId == basicmemberRole.Id);
                    if(roleGroup == null)
                    {
                        _roleGroupRepository!.Add(new ApplicationRoleGroup
                        {
                            RoleId = basicmemberRole.Id.ToString(),
                            GroupId = customerGr.ID
                        });
                    }                 
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Occurred error when AddRoleGroups: ", ex.ToString());
                return false;
            }
            return true;
        }

        private async Task<bool> AddMaintenanceUsers()
        {
            try
            {
                var adminUser = await _userManager!.FindByNameAsync(managerUsername);
                if (adminUser == null)
                {
                    var ret = await _userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = managerUsername,
                        Email = managerEmail,
                        EmailConfirmed = true,
                    }, managerPassword);
                    if (ret != null && ret.Succeeded == false)
                    {
                        _logger.LogError(ret.Errors.ToList().ToString());
                        return false;
                    }
                }

                var workerUser = await _userManager!.FindByNameAsync(workerUsername);
                if (workerUser == null)
                {
                    var ret = await _userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = workerUsername,
                        Email = workerEmail,
                        EmailConfirmed = true,
                    }, workerPassword);
                    if (ret != null && ret.Succeeded == false)
                    {
                        _logger.LogError(ret.Errors.ToList().ToString());
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Occurred error when AddMaintenanceUsers: ", ex.ToString());
                return false;
            }
            return true;
        }

        private async Task<bool> AddUserGroups()
        {
            try
            {
                // admin
                var maintenanceGr = _groupRepository!.GetSingleByCondition(x => x.Name == Group.Maintenancer);
                var adminUser = await _userManager!.FindByNameAsync(managerUsername);
                if (maintenanceGr != null && adminUser != null)
                {
                    var userGroup = _userGroupRepository!.GetSingleByCondition(x => x.UserId == adminUser.Id && x.GroupId == maintenanceGr.ID);
                    if(userGroup == null)
                    {
                        _userGroupRepository!.Add(new ApplicationUserGroup
                        {
                            GroupId = maintenanceGr.ID,
                            UserId = adminUser.Id
                        });
                    }                
                }
                // worker
                var workerUser = await _userManager!.FindByNameAsync(workerUsername);
                if (maintenanceGr != null && workerUser != null)
                {
                    var userGroup = _userGroupRepository!.GetSingleByCondition(x => x.UserId == workerUser.Id && x.GroupId == maintenanceGr.ID);
                    if (userGroup == null)
                        _userGroupRepository!.Add(new ApplicationUserGroup
                    {
                        GroupId = maintenanceGr.ID,
                        UserId = workerUser.Id
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Occurred error when AddUserGroups: ", ex.ToString());
                return false;
            }
            return true;
        }

        private async Task<bool> AddUserRoles()
        {
            try
            {
                // admin
                var adminUser = await _userManager!.FindByNameAsync(managerUsername);
                var adminRole = await _roleManager!.FindByNameAsync(Role.Admin);
                var workerRole = await _roleManager!.FindByNameAsync(Role.Worker);
                if (adminUser != null && adminRole != null)
                {
                    await _userManager.AddToRoleAsync(adminUser, adminRole.Name);
                }
                // worker
                var workerUser = await _userManager!.FindByNameAsync(workerUsername);
                if (workerUser != null && workerRole != null)
                {
                    await _userManager.AddToRoleAsync(workerUser, workerRole.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Occurred error when AddUserRoles: ", ex.ToString());
                return false;
            }

            return true;
        }

    }
}
