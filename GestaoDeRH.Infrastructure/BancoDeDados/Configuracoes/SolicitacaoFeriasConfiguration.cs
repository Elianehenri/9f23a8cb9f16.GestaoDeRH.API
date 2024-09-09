using GestaoDeRH.Dominio.Ferias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeRH.Infra.BancoDeDados.Configuracoes
{
    public  class SolicitacaoFeriasConfiguration : IEntityTypeConfiguration<SolicitacaoFerias>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoFerias> builder)
        {

            builder.HasKey(x => x.Id);


            builder.Property(x => x.DataInicioFerias)
                   .IsRequired();

            builder.Property(x => x.DataFimFerias)
                   .IsRequired();

            builder.Property(x => x.DataSolicitacao)
                   .IsRequired();




            builder.HasOne(x => x.Colaborador)
                   .WithMany()
                   .HasForeignKey(x => x.ColaboradorId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.ToTable("Ferias");
        }
    }
}
