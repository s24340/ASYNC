﻿namespace async.Services;
using async.Models;
public interface IWarehouseDb
{
    Task<int> PostWarehouse(Warehouse warehouse);
}