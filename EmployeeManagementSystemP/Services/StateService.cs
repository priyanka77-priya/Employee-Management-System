using EmployeeManagementSystemP.Data;
using EmployeeManagementSystemP.Models;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystemP.DTOs;

namespace EmployeeManagementSystemP.Services
{
    public class StateService : IStateService
    {
        private readonly AppDbContext _context;

        public StateService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<State>> GetAllStatesAsync()
        {
            return await _context.states.ToListAsync();
        }

        public async Task<State> AddStateAsync(string stateName, int countryId)
        {
            var state = new State
            {
                StateName = stateName,
                CountryId = countryId
            };

            _context.states.Add(state);
            await _context.SaveChangesAsync();

            return state;
        }

        public async Task AddStatesBulkAsync(List<StateInputModel> states)
        {
            var stateEntities = states.Select(s => new State
            {
                StateName = s.StateName,
                CountryId = s.CountryId
            }).ToList();

            _context.states.AddRange(stateEntities);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteStateByIdAsync(int id)
        {
            var state = await _context.states.FindAsync(id);
            if (state == null)
                return false;

            _context.states.Remove(state);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
