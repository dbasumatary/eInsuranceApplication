using E_Insurance_App.EmailService.Interface;
using E_Insurance_App.Models.DTOs;
using E_Insurance_App.Models.Entities;
using E_Insurance_App.Repositories.Interface;
using E_Insurance_App.Services.Interface;
using E_Insurance_App.Utilities.Interface;
using Newtonsoft.Json;

namespace E_Insurance_App.Services.Implementation
{
    public class AgentService : IAgentService
    {
        private readonly IAgentRepository _agentRepository;
        private readonly ILogger<AgentService> _logger;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRabbitMqService _rabbitMqService;

        public AgentService(IAgentRepository agentRepository, ILogger<AgentService> logger, IPasswordHasher passwordHasher, IRabbitMqService rabbitMqService)
        {
            _agentRepository = agentRepository;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _rabbitMqService = rabbitMqService;
        }

        public async Task<Agent> RegisterAgentAsync(AgentRegisterDTO agentDto)
        {
            _logger.LogInformation("Registering new agent with username: {Username}", agentDto.Username);

            try
            {
                var plainPassword = agentDto.Password;

                var agent = new Agent
                {
                    Username = agentDto.Username,
                    Password = _passwordHasher.HashPassword(agentDto.Password),
                    Email = agentDto.Email,
                    FullName = agentDto.FullName,
                    CommissionRate = agentDto.CommissionRate
                };

                var regAgent = await _agentRepository.RegisterAgentAsync(agent);

                _logger.LogInformation("Agent registered successfully with ID: {AgentID}", regAgent.AgentID);

                var emailMessage = new
                {
                    Email = regAgent.Email,
                    Subject = "Welcome to Insurance Company - Here are your Account Credentials",
                    Body = $@"
                        <p>Dear {regAgent.FullName},</p>
                        <p>Welcome to <strong>Insurance Company</strong>. Your Insurance Agent account has been successfully created.</p>
                        <p><strong>Login Credentials:</strong></p>
                        <ul>
                            <li><strong>Username:</strong> {regAgent.Username}</li>
                            <li><strong>Password:</strong> {plainPassword}</li>
                        </ul>
                        <p>For security reasons, please change your password after logging in.</p>
                        <p>If you did not request this account, please contact our support team immediately.</p>
                        <p>Best regards,<br/><strong>Insurance Company Support Team</strong></p>
                    "
                };

                _rabbitMqService.SendMessage("emailQueue", JsonConvert.SerializeObject(emailMessage));

                return regAgent;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during register agent with username: {Username}", agentDto.Username);
                throw new Exception($"Error registering agents: {ex.Message}");
            }
        }


        public async Task<Agent?> GetAgentByIdAsync(int agentId)
        {
            _logger.LogInformation("Retrieving agent by ID: {AgentID}", agentId);

            try
            {
                var agent = await _agentRepository.GetAgentByIdAsync(agentId);
                if (agent == null)
                {
                    _logger.LogWarning("Agent not found with ID: {AgentID}", agentId);
                }

                return agent;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during getting agent by ID: {AgentID}", agentId);
                throw new Exception($"Error getting agents by Id: {ex.Message}");
            }
        }


        public async Task<IEnumerable<Agent>> GetAllAgentsAsync()
        {
            _logger.LogInformation("Retrieving list of all agents.");

            try
            {
                var agents = await _agentRepository.GetAllAgentsAsync();
                _logger.LogInformation("Retrieved {AgentCount} agents.", agents.Count());

                return agents;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during getting all agents.");
                throw new Exception($"Error getting  agents list: {ex.Message}");
            }
        }


        public async Task<Agent?> UpdateAgentAsync(int agentId, AgentUpdateDTO agentDto)
        {
            _logger.LogInformation("Updating agent with ID: {AgentID}", agentId);

            try
            {
                var agent = await _agentRepository.GetAgentByIdAsync(agentId);
                if (agent == null)
                {
                    _logger.LogWarning("Agent not found with ID: {AgentID}", agentId);
                    return null;
                }

                agent.FullName = agentDto.FullName;
                agent.Email = agentDto.Email;
                agent.CommissionRate = agentDto.CommissionRate;

                var updatedAgent = await _agentRepository.UpdateAgentAsync(agent);

                _logger.LogInformation("Agent updated successfully with ID: {AgentID}", updatedAgent.AgentID);
                return updatedAgent;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during updating agent with ID: {AgentID}", agentId);
                throw new Exception($"Error updating agent: {ex.Message}");
            }
        }


        public async Task<bool> DeleteAgentAsync(int agentId)
        {
            _logger.LogInformation("Deleting agent with ID: {AgentID}", agentId);

            try
            {
                var result = await _agentRepository.DeleteAgentAsync(agentId);

                if (result)
                {
                    _logger.LogInformation("Agent deleted successfully with ID: {AgentID}", agentId);
                }
                else
                {
                    _logger.LogWarning("Failed to delete agent with ID: {AgentID}", agentId);
                }
                return result;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during deleting agent with ID: {AgentID}", agentId);
                throw new Exception($"Error deleting agent: {ex.Message}");
            }
        }
    }
}
