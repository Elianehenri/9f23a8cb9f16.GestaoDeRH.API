using GestaoDeRH.Dominio.Ferias;
using GestaoDeRH.Dominio.Interfaces;
using GestaoDeRH.Dominio.Pessoas;
using GestaoDeRH.Aplicacao.Ferias;
using Moq;
using GestaoDeRH.Aplicacao.Ferias.DTO;


namespace GestaoDeRh.Tests.FeriasTests
{
    public class SolicitacaoFeriasTests
    {
        private readonly Mock<IRepositorio<SolicitacaoFerias>> _repositorioSolicitarFeriasMock;
        private readonly Mock<IRepositorio<Colaborador>> _repositorioColaboradoresMock;
        private readonly SolicitacaoFeriasServico _solicitarFeriasService;

        public SolicitacaoFeriasTests()
        {
            _repositorioSolicitarFeriasMock = new Mock<IRepositorio<SolicitacaoFerias>>();
            _repositorioColaboradoresMock = new Mock<IRepositorio<Colaborador>>();
            _solicitarFeriasService = new SolicitacaoFeriasServico(
            _repositorioSolicitarFeriasMock.Object,
            _repositorioColaboradoresMock.Object
            );
        }

        [Fact]
        public async Task SolicitarFerias_ColaboradorNaoEncontrado_DeveRetornarErro()
        {
            // Arrange
            var dto = new SolicitacaoFeriasDto { ColaboradorId = 1 };

            _repositorioColaboradoresMock
                .Setup(repo => repo.Obter(dto.ColaboradorId))
                .ReturnsAsync((Colaborador)null);

            // Act
            var resultado = await _solicitarFeriasService.SolicitarFerias(dto);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Contains("Colaborador não encontrado.", resultado.Erros);
        }

        [Fact]
        public async Task SolicitarFerias_ColaboradorNaoCompletouUmAno_DeveRetornarErro()
        {
            // Arrange
            var colaborador = new Colaborador
            {
                Id = 1,
                DataInicioContratoDeTrabalho = DateTime.Now.AddMonths(-6)
            };

            var dto = new SolicitacaoFeriasDto
            {
                ColaboradorId = colaborador.Id,
                DataInicioFerias = DateTime.Now.AddMonths(1)
            };

            _repositorioColaboradoresMock
                .Setup(repo => repo.Obter(dto.ColaboradorId))
                .ReturnsAsync(colaborador);

            // Act
            var resultado = await _solicitarFeriasService.SolicitarFerias(dto);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Contains("Colaborador ainda não completou um ano de contrato.", resultado.Erros[0]);
        }

        [Fact]
        public async Task SolicitarFerias_JaExisteSolicitacaoNoPeriodo_DeveRetornarErro()
        {
            // Arrange
            var colaborador = new Colaborador { Id = 1, DataInicioContratoDeTrabalho = DateTime.Now.AddYears(-2) };

            var solicitacaoExistente = new SolicitacaoFerias
            {
                ColaboradorId = colaborador.Id,
                DataInicioFerias = DateTime.Now.AddDays(10),
                DataFimFerias = DateTime.Now.AddDays(20)
            };

            var dto = new SolicitacaoFeriasDto
            {
                ColaboradorId = colaborador.Id,
                DataInicioFerias = DateTime.Now.AddDays(15),
                DataFimFerias = DateTime.Now.AddDays(25)
            };

            _repositorioColaboradoresMock
                .Setup(repo => repo.Obter(dto.ColaboradorId))
                .ReturnsAsync(colaborador);

            _repositorioSolicitarFeriasMock
                .Setup(repo => repo.Listar())
                .ReturnsAsync(new List<SolicitacaoFerias> { solicitacaoExistente });

            // Act
            var resultado = await _solicitarFeriasService.SolicitarFerias(dto);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Contains("Já há uma solicitação neste período para este colaborador.", resultado.Erros);
        }

        [Fact]
        public async Task SolicitarFerias_ExcedeLimiteDeDiasNoAno_DeveRetornarErro()
        {
            // Arrange
            var colaborador = new Colaborador { Id = 1, DataInicioContratoDeTrabalho = DateTime.Now.AddYears(-2) };

            var solicitacoesExistentes = new List<SolicitacaoFerias>
            {
                new SolicitacaoFerias
                {
                    ColaboradorId = colaborador.Id,
                    DataInicioFerias = new DateTime(DateTime.Now.Year, 1, 1),
                    DataFimFerias = new DateTime(DateTime.Now.Year, 1, 15)
                },
                new SolicitacaoFerias
                {
                    ColaboradorId = colaborador.Id,
                    DataInicioFerias = new DateTime(DateTime.Now.Year, 2, 1),
                    DataFimFerias = new DateTime(DateTime.Now.Year, 2, 15)
                }
            };

            var dto = new SolicitacaoFeriasDto
            {
                ColaboradorId = colaborador.Id,
                DataInicioFerias = new DateTime(DateTime.Now.Year, 3, 1),
                DataFimFerias = new DateTime(DateTime.Now.Year, 3, 15)
            };

            _repositorioColaboradoresMock
                .Setup(repo => repo.Obter(dto.ColaboradorId))
                .ReturnsAsync(colaborador);

            _repositorioSolicitarFeriasMock
                .Setup(repo => repo.Listar())
                .ReturnsAsync(solicitacoesExistentes);

            // Act
            var resultado = await _solicitarFeriasService.SolicitarFerias(dto);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Contains("Total de dias solicitados:", resultado.Erros[0]);
        }

    }
}
