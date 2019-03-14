using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpWars.Common.Extensions;
using CSharpWars.DataAccess.Repositories.Interfaces;
using CSharpWars.DtoModel;
using CSharpWars.Logic.Interfaces;
using CSharpWars.Mapping.Interfaces;
using CSharpWars.Model;

namespace CSharpWars.Logic
{
    public class PlayerLogic : IPlayerLogic
    {
        private readonly IRepository<Player> _playerRepository;
        private readonly IMapper<Player, PlayerDto> _playerMapper;

        public PlayerLogic(IRepository<Player> playerRepository, IMapper<Player, PlayerDto> playerMapper)
        {
            _playerRepository = playerRepository;
            _playerMapper = playerMapper;
        }

        public async Task<IList<PlayerDto>> GetAllPlayers()
        {
            var players = await _playerRepository.GetAll();
            return _playerMapper.Map(players);
        }

        public async Task<PlayerDto> Login(LoginDto login)
        {
            (String Salt, String Hashed) hashedPassword;
            var existingPlayer = await _playerRepository.Single(x => x.Name == login.Name);
            if (existingPlayer == null)
            {
                hashedPassword = login.Secret.HashPassword();
                var newPlayer = new Player
                {
                    Name = login.Name,
                    Salt = hashedPassword.Salt,
                    Hashed = hashedPassword.Hashed
                };
                newPlayer = await _playerRepository.Create(newPlayer);
                return _playerMapper.Map(newPlayer);
            }

            hashedPassword = login.Secret.HashPassword(existingPlayer.Salt);
            if (existingPlayer.Hashed == hashedPassword.Hashed)
            {
                return _playerMapper.Map(existingPlayer);
            }

            return null;
        }
    }
}