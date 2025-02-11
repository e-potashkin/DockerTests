﻿using DockerTestsSample.Store.Entities;

namespace DockerTestsSample.Repositories.People;

public interface IPersonRepository: IDbRepository
{
    Task<IReadOnlyCollection<Person>> GetAllAsync(CancellationToken ct = default);
    
    Task<Person?> GetAsync(Guid id, CancellationToken ct = default);

    Task CreateAsync(Person person, CancellationToken ct = default);

    Task UpdateAsync(Person person, CancellationToken ct = default);
    
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}