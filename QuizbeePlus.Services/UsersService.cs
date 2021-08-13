using QuizbeePlus.Data;
using QuizbeePlus.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizbeePlus.Services
{
    public class UsersService
    {
        #region Define as Singleton
        private static UsersService _Instance;

        public static UsersService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new UsersService();
                }

                return (_Instance);
            }
        }

        private UsersService()
        {
        }
        #endregion
        
        public UsersSearch GetUsersWithRoles(string searchTerm, int pageNo, int pageSize)
        {
            using (var context = new QuizbeeContext())
            {
                var search = new UsersSearch();

                if (string.IsNullOrEmpty(searchTerm))
                {
                    search.Users = context.Users
                                        .Include(u => u.Roles)
                                        .OrderByDescending(x => x.RegisteredOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .Select(x => new UserWithRoleEntity() { User = x, Roles = x.Roles.Select(r => context.Roles.Where(role => role.Id == r.RoleId).FirstOrDefault())
                                        .ToList() }).ToList();

                    search.TotalCount = context.Users.Count();
                }
                else
                {
                    search.Users = context.Users
                                        .Where(u => u.UserName.ToLower().Contains(searchTerm.ToLower()))
                                        .Include(u => u.Roles)
                                        .OrderByDescending(x => x.RegisteredOn)
                                        .Skip((pageNo - 1) * pageSize)
                                        .Take(pageSize)
                                        .Select(x => new UserWithRoleEntity() { User = x, Roles = x.Roles.Select(r => context.Roles.Where(role => role.Id == r.RoleId).FirstOrDefault())
                                        .ToList() }).ToList();

                    search.TotalCount = context.Users
                                        .Where(u => u.UserName.ToLower().Contains(searchTerm.ToLower())).Count();
                }

                return search;
            }
        }
        
        public UserWithRoleEntity GetUserWithRolesByID(string userID)
        {
            using (var context = new QuizbeeContext())
            {
                return context.Users
                                    .Where(x => x.Id == userID)
                                    .Include(u => u.Roles)
                                    .Select(x => new UserWithRoleEntity()
                                    {
                                        User = x,
                                        Roles = x.Roles.Select(r => context.Roles.Where(role => role.Id == r.RoleId).FirstOrDefault()).ToList()
                                    }).FirstOrDefault();
            }
        }
    }
}
