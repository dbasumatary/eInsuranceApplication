using E_Insurance_App.Data;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace E_Insurance_App.Repositories.Implementation
{
    public class AgentRepository : IAgentRepository
    {
        private readonly InsuranceDbContext _context;
        private readonly ILogger<AgentRepository> _logger;

        public AgentRepository(InsuranceDbContext context, ILogger<AgentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Agent> RegisterAgentAsync(Agent agent)
        {
            try
            {
                _context.Agents.Add(agent);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Agent registered: {agent.Username}");
                return agent;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error registering agent: {ex.Message}");
                throw;
            }
        }


        public async Task<Agent?> GetAgentByIdAsync(int agentId)
        {
            try
            {
                return await _context.Agents.FindAsync(agentId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting agent: {ex.Message}");
                throw;
            }
        }


        public async Task<IEnumerable<Agent>> GetAllAgentsAsync()
        {
            try
            {
                return await _context.Agents.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting list of agents: {ex.Message}");
                throw;
            }
        }


        public async Task<Agent?> UpdateAgentAsync(Agent agent)
        {
            try
            {
                var existingAgent = await _context.Agents.FindAsync(agent.AgentID);
                if (existingAgent == null)
                {
                    return null;
                }

                existingAgent.FullName = agent.FullName;
                existingAgent.Email = agent.Email;
                existingAgent.CommissionRate = agent.CommissionRate;

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Agent updated: {agent.Username}");
                return existingAgent;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating agent: {ex.Message}");
                throw;
            }
        }


        public async Task<bool> DeleteAgentAsync(int agentId)
        {
            try
            {
                var agent = await _context.Agents.FindAsync(agentId);
                if (agent == null)
                {
                    return false;
                }

                _context.Agents.Remove(agent);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Agent deleted: {agent.Username}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting agent: {ex.Message}");
                throw;
            }
        }
    }
}
