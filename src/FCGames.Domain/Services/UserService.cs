using FCGames.Domain.Entities;
using FCGames.Domain.Interfaces;
using FCGames.Domain.Interfaces.Infraestructure;

namespace FCGames.Domain.Services;

public class UserService(IUserRepository userRepository, UserData userData) : BaseService<User>(userRepository, userData), IUserService
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<User> GetById(Guid id)
    {
        return await _userRepository.GetById(id, false, false);
    }

    public override async Task<User> Add(User entity)
    {
        var user = await _userRepository.GetByEmail(entity.Email);

        if (user != null)
            throw new Exception("O usuário já existe.");

        entity.PrepareToInsert(_userData.Id);
        
        return await base.Add(entity);
    }

    public async Task<User> GetByEmail(string? email)
    {
        return await _userRepository.GetByEmail(email);
    }
}