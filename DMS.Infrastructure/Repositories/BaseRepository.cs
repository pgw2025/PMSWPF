using System.Diagnostics;
using System.Linq.Expressions;
using DMS.Core.Helper;
using DMS.Infrastructure.Data;
using SqlSugar;

namespace DMS.Infrastructure.Repositories;

/// <summary>
///     通用仓储基类，封装了对实体对象的常用 CRUD 操作。
/// </summary>
/// <typeparam name="TEntity">实体类型，必须是引用类型且具有无参构造函数。</typeparam>
public abstract class BaseRepository<TEntity>
    where TEntity : class, new()
{
    private readonly SqlSugarDbContext _dbContext;


    /// <summary>
    ///     初始化 BaseRepository 的新实例。
    /// </summary>
    /// <param name="dbContext">事务管理对象，通过依赖注入提供。</param>
    protected BaseRepository(SqlSugarDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    ///     获取当前事务的 SqlSugarClient 实例，用于数据库操作。
    /// </summary>
    protected SqlSugarClient Db
    {
        get { return _dbContext.GetInstance(); }
    }

    /// <summary>
    ///     异步添加一个新实体。
    /// </summary>
    /// <param name="entity">要添加的实体对象。</param>
    /// <returns>返回已添加的实体对象（可能包含数据库生成的主键等信息）。</returns>
    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var result = await Db.Insertable(entity)
                             .ExecuteReturnEntityAsync();
        stopwatch.Stop();
        NlogHelper.Info($"Add {typeof(TEntity).Name}耗时：{stopwatch.ElapsedMilliseconds}ms");
        return result;
    }

    /// <summary>
    ///     异步更新一个现有实体。
    /// </summary>
    /// <param name="entity">要更新的实体对象。</param>
    /// <returns>返回受影响的行数。</returns>
    public virtual async Task<int> UpdateAsync(TEntity entity)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var result = await Db.Updateable(entity)
                             .ExecuteCommandAsync();
        stopwatch.Stop();
        NlogHelper.Info($"Update {typeof(TEntity).Name}耗时：{stopwatch.ElapsedMilliseconds}ms");
        return result;
    }

    /// <summary>
    ///     异步删除一个实体。
    /// </summary>
    /// <param name="entity">要删除的实体对象。</param>
    /// <returns>返回受影响的行数。</returns>
    public virtual async Task<int> DeleteAsync(TEntity entity)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var result = await Db.Deleteable(entity)
                             .ExecuteCommandAsync();
        stopwatch.Stop();
        NlogHelper.Info($"Delete {typeof(TEntity).Name}耗时：{stopwatch.ElapsedMilliseconds}ms");
        return result;
    }
    

    /// <summary>
    ///     异步获取所有实体。
    /// </summary>
    /// <returns>返回包含所有实体的列表。</returns>
    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var entities = await Db.Queryable<TEntity>()
                               .ToListAsync();
        stopwatch.Stop();
        NlogHelper.Info($"GetAll {typeof(TEntity).Name}耗时：{stopwatch.ElapsedMilliseconds}ms");
        return entities;
    }


    /// <summary>
    ///     异步根据主键 ID 获取单个实体。
    /// </summary>
    /// <param name="id">实体的主键 ID。</param>
    /// <returns>返回找到的实体，如果未找到则返回 null。</returns>
    public virtual async Task<TEntity> GetByIdAsync(int id)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var entity = await Db.Queryable<TEntity>()
                             .In(id)
                             .FirstAsync();
        stopwatch.Stop();
        NlogHelper.Info($"GetById {typeof(TEntity).Name}耗时：{stopwatch.ElapsedMilliseconds}ms");
        return entity;
    }

    /// <summary>
    ///     异步根据指定条件获取单个实体。
    /// </summary>
    /// <param name="expression">查询条件的 Lambda 表达式。</param>
    /// <returns>返回满足条件的第一个实体，如果未找到则返回 null。</returns>
    public virtual async Task<TEntity> GetByConditionAsync(Expression<Func<TEntity, bool>> expression)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var entity = await Db.Queryable<TEntity>()
                             .FirstAsync(expression);
        stopwatch.Stop();
        NlogHelper.Info($"GetByCondition {typeof(TEntity).Name}耗时：{stopwatch.ElapsedMilliseconds}ms");
        return entity;
    }

    /// <summary>
    ///     异步判断是否存在满足条件的实体。
    /// </summary>
    /// <param name="expression">查询条件的 Lambda 表达式。</param>
    /// <returns>如果存在则返回 true，否则返回 false。</returns>
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var result = await Db.Queryable<TEntity>()
                             .AnyAsync(expression);
        stopwatch.Stop();
        NlogHelper.Info($"Exists {typeof(TEntity).Name}耗时：{stopwatch.ElapsedMilliseconds}ms");
        return result;
    }

    public async Task BeginTranAsync()
    {
        await Db.BeginTranAsync();
    }

    public async Task CommitTranAsync()
    {
        await Db.CommitTranAsync();
    }


    public async Task RollbackTranAsync()
    {
        await Db.RollbackTranAsync();
    }
}