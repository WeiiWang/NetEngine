using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.RedisLock.Core
{
    /// <summary>
    /// �ṩ������ <see cref="ReaderWriterLock"/> �ķֲ�ʽ��������
    /// </summary>
    public interface IDistributedReaderWriterLock
    {
        /// <summary>
        /// A name that uniquely identifies the lock
        /// </summary>
        string Name { get; }

        /// <summary>
        /// ����ͬ����ȡ READ ���� �������Ķ����� �� WRITE �������ݡ� �÷���
        /// <code>
        ///     using (var handle = myLock.TryAcquireReadLock(...))
        ///     {
        ///         if (handle != null) { /* we have the lock! */ }
        ///     }
        ///     // dispose releases the lock if we took it
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ 0</param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ�� <see cref="IDistributedSynchronizationHandle"/> �������ͷ�������ʧ��ʱΪ��</returns>
        IDistributedSynchronizationHandle? TryAcquireReadLock(TimeSpan timeout = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// ͬ����ȡ READ ����������Գ�ʱ����ʧ�ܲ����� <see cref="TimeoutException"/>�� �������Ķ����� �� WRITE �������ݡ� �÷���
        /// <code>
        ///     using (myLock.AcquireReadLock(...))
        ///     {
        ///         /* we have the lock! */
        ///     }
        ///     // dispose releases the lock
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ <see cref="Timeout.InfiniteTimeSpan"/></param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ��<see cref="IDistributedSynchronizationHandle"/>���������ͷ���</returns>
        IDistributedSynchronizationHandle AcquireReadLock(TimeSpan? timeout = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// �����첽��ȡ READ ���� �������Ķ����� �� WRITE �������ݡ� �÷���
        /// <code>
        ///     await using (var handle = await myLock.TryAcquireReadLockAsync(...))
        ///     {
        ///         if (handle != null) { /* we have the lock! */ }
        ///     }
        ///     // dispose releases the lock if we took it
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ 0</param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ�� <see cref="IDistributedSynchronizationHandle"/> �������ͷ�������ʧ��ʱΪ��</returns>
        ValueTask<IDistributedSynchronizationHandle?> TryAcquireReadLockAsync(TimeSpan timeout = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// �첽��ȡ READ ����������Գ�ʱ����ʧ�ܲ����� <see cref="TimeoutException"/>�� �������Ķ����� �� WRITE �������ݡ� �÷���
        /// <code>
        ///     await using (await myLock.AcquireReadLockAsync(...))
        ///     {
        ///         /* we have the lock! */
        ///     }
        ///     // dispose releases the lock
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ <see cref="Timeout.InfiniteTimeSpan"/></param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ��<see cref="IDistributedSynchronizationHandle"/>���������ͷ���</returns>
        ValueTask<IDistributedSynchronizationHandle> AcquireReadLockAsync(TimeSpan? timeout = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// ����ͬ����ȡ WRITE ���� ����һ�� WRITE ���� UPGRADE �������ݡ� �÷���
        /// <code>
        ///     using (var handle = myLock.TryAcquireWriteLock(...))
        ///     {
        ///         if (handle != null) { /* we have the lock! */ }
        ///     }
        ///     // dispose releases the lock if we took it
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ 0</param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ�� <see cref="IDistributedSynchronizationHandle"/> �������ͷ�������ʧ��ʱΪ��</returns>
        IDistributedSynchronizationHandle? TryAcquireWriteLock(TimeSpan timeout = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// ͬ����ȡ WRITE ����������Գ�ʱ����ʧ�ܲ����� <see cref="TimeoutException"/>�� ����һ�� WRITE ���� UPGRADE �������ݡ� �÷���
        /// <code>
        ///     using (myLock.AcquireWriteLock(...))
        ///     {
        ///         /* we have the lock! */
        ///     }
        ///     // dispose releases the lock
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ <see cref="Timeout.InfiniteTimeSpan"/></param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ��<see cref="IDistributedSynchronizationHandle"/>���������ͷ���</returns>
        IDistributedSynchronizationHandle AcquireWriteLock(TimeSpan? timeout = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// �����첽��ȡ WRITE ���� ����һ�� WRITE ���� UPGRADE �������ݡ� �÷���
        /// <code>
        ///     await using (var handle = await myLock.TryAcquireWriteLockAsync(...))
        ///     {
        ///         if (handle != null) { /* we have the lock! */ }
        ///     }
        ///     // dispose releases the lock if we took it
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ 0</param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ�� <see cref="IDistributedSynchronizationHandle"/> �������ͷ�������ʧ��ʱΪ��</returns>
        ValueTask<IDistributedSynchronizationHandle?> TryAcquireWriteLockAsync(TimeSpan timeout = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// �첽��ȡ WRITE ����������Գ�ʱ����ʧ�ܲ����� <see cref="TimeoutException"/>�� ����һ�� WRITE ���� UPGRADE �������ݡ� �÷���
        /// <code>
        ///     await using (await myLock.AcquireWriteLockAsync(...))
        ///     {
        ///         /* we have the lock! */
        ///     }
        ///     // dispose releases the lock
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ <see cref="Timeout.InfiniteTimeSpan"/></param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ��<see cref="IDistributedSynchronizationHandle"/>���������ͷ���</returns>
        ValueTask<IDistributedSynchronizationHandle> AcquireWriteLockAsync(TimeSpan? timeout = null, CancellationToken cancellationToken = default);
    }
}
