using Eventos.IO.Domain.Eventos;
using Eventos.IO.Infra.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventos.IO.Infra.Data.Mappings
{
	public class EnderecoMapping : EntityTypeConfiguration<Endereco>
	{
		public override void Map(EntityTypeBuilder<Endereco> builder)
		{
			builder.Property(e => e.Logradouro)
				.IsRequired()
				.HasMaxLength(150)
				.HasColumnType("varchar(150)");

			builder.Property(e => e.Numero)
				.IsRequired()
				.HasMaxLength(20)
				.HasColumnType("varchar(20)");

			builder.Property(e => e.Bairro)
				.IsRequired()
				.HasMaxLength(50)
				.HasColumnType("varchar(50)");

			builder.Property(e => e.Cep)
				.IsRequired()
				.HasMaxLength(8)
				.HasColumnType("varchar(8)");

			builder.Property(e => e.Complemento)
				.HasMaxLength(100)
				.HasColumnType("varchar(100)");

			builder.Property(e => e.Cidade)
				.IsRequired()
				.HasMaxLength(100)
				.HasColumnType("varchar(100)");

			builder.Property(e => e.Estado)
				.IsRequired()
				.HasMaxLength(100)
				.HasColumnType("varchar(100)");
			
			builder.Ignore(end => end.ValidationResult);

			builder.Ignore(end => end.CascadeMode);

			builder
				.HasOne(end => end.Evento)
				.WithOne(e => e.Endereco)
				.HasForeignKey<Endereco>(end => end.EventoId)
				.IsRequired(false);

			builder.ToTable("Enderecos");
		}
	}
}