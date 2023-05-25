﻿namespace Infrastructure.Services.Interfaces
{
    public interface IService <T, TCreate, TUpdate>
    {
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllRawAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> CreateAsync(TCreate dto);
        Task<T> UpdateAsync(TUpdate dto);
        Task DeleteAsync(T entity);
    }
}
