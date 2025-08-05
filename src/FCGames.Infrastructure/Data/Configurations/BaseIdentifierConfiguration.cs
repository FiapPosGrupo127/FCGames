using FCGames.Domain.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCGames.Infrastructure.Data.Configurations;

public abstract class BaseIdentifierConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IIdentifier
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        
        // Configuração do tipo Guid que funciona para ambos os providers
        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .IsRequired();
    }
}