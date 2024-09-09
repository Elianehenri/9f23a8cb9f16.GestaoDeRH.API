using GestaoDeRH.Aplicacao.Colaboradores.DTO;
using GestaoDeRH.Aplicacao.Colaboradores;
using GestaoDeRH.Dominio.Interfaces;
using GestaoDeRH.Dominio.Pessoas;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeRh.Tests.ColaboradorTests
{
    public class ColaboradorEmailValidationTests
    {
        private readonly Mock<IRepositorio<Colaborador>> _repositorioColaboradorMock;
        private readonly ColaboradorServico _colaboradorService;

        public ColaboradorEmailValidationTests()
        {
            _repositorioColaboradorMock = new Mock<IRepositorio<Colaborador>>();
            _colaboradorService = new ColaboradorServico(_repositorioColaboradorMock.Object);
        }

        [Fact]
        public async Task CriarColaborador_EmailInvalido_DeveRetornarErro()
        {
            // Arrange
            var colaboradorDto = new ColaboradorDTO
            {
                Nome = "João",
                Sobrenome = "Silva",
                Email = "email-invalido",
                CPF = "123.456.789-00",
                RG = "12.345.678-9",
                DataDeNascimento = DateTime.Now.AddYears(-30),
                DataInicioContratoDeTrabalho = DateTime.Now.AddYears(-1)
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _colaboradorService.CriarColaborador(colaboradorDto));
            Assert.Equal("O email informado não é válido.", exception.Message);
        }

        [Fact]
        public async Task CriarColaborador_EmailJaCadastrado_DeveRetornarErro()
        {
            // Arrange
            var colaboradorDto = new ColaboradorDTO
            {
                Nome = "João",
                Sobrenome = "Silva",
                Email = "joao.silva@email.com",
                CPF = "123.456.789-00",
                RG = "12.345.678-9",
                DataDeNascimento = DateTime.Now.AddYears(-30),
                DataInicioContratoDeTrabalho = DateTime.Now.AddYears(-1)
            };

            var colaboradorExistente = new Colaborador
            {
                Nome = "Maria",
                Sobrenome = "Oliveira",
                Email = colaboradorDto.Email,  // Mesmo e-mail já cadastrado
                CPF = "987.654.321-00"
            };

            _repositorioColaboradorMock
                .Setup(repo => repo.Listar())
                .ReturnsAsync(new[] { colaboradorExistente }.ToList());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _colaboradorService.CriarColaborador(colaboradorDto));
            Assert.Equal("O email já está cadastrado para outro colaborador.", exception.Message);
        }


    }
}
