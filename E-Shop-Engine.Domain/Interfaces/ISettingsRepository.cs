﻿using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface ISettingsRepository
    {
        Settings Get();
        void Update(Settings entity);
    }
}
