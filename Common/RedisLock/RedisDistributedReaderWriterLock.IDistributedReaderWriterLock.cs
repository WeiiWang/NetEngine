using Common.RedisLock.Core;
using Common.RedisLock.Core.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.RedisLock
{
    public partial class RedisDistributedReaderWriterLock
    {
        // �Զ�����

        IDistributedSynchronizationHandle? IDistributedReaderWriterLock.TryAcquireReadLock(TimeSpan timeout, CancellationToken cancellationToken) =>
            this.TryAcquireReadLock(timeout, cancellationToken);
        IDistributedSynchronizationHandle IDistributedReaderWriterLock.AcquireReadLock(TimeSpan? timeout, CancellationToken cancellationToken) =>
            this.AcquireReadLock(timeout, cancellationToken);
        ValueTask<IDistributedSynchronizationHandle?> IDistributedReaderWriterLock.TryAcquireReadLockAsync(TimeSpan timeout, CancellationToken cancellationToken) =>
            this.TryAcquireReadLockAsync(timeout, cancellationToken).Convert(To<IDistributedSynchronizationHandle?>.ValueTask);
        ValueTask<IDistributedSynchronizationHandle> IDistributedReaderWriterLock.AcquireReadLockAsync(TimeSpan? timeout, CancellationToken cancellationToken) =>
            this.AcquireReadLockAsync(timeout, cancellationToken).Convert(To<IDistributedSynchronizationHandle>.ValueTask);
        IDistributedSynchronizationHandle? IDistributedReaderWriterLock.TryAcquireWriteLock(TimeSpan timeout, CancellationToken cancellationToken) =>
            this.TryAcquireWriteLock(timeout, cancellationToken);
        IDistributedSynchronizationHandle IDistributedReaderWriterLock.AcquireWriteLock(TimeSpan? timeout, CancellationToken cancellationToken) =>
            this.AcquireWriteLock(timeout, cancellationToken);
        ValueTask<IDistributedSynchronizationHandle?> IDistributedReaderWriterLock.TryAcquireWriteLockAsync(TimeSpan timeout, CancellationToken cancellationToken) =>
            this.TryAcquireWriteLockAsync(timeout, cancellationToken).Convert(To<IDistributedSynchronizationHandle?>.ValueTask);
        ValueTask<IDistributedSynchronizationHandle> IDistributedReaderWriterLock.AcquireWriteLockAsync(TimeSpan? timeout, CancellationToken cancellationToken) =>
            this.AcquireWriteLockAsync(timeout, cancellationToken).Convert(To<IDistributedSynchronizationHandle>.ValueTask);

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
        /// <returns>һ�� <see cref="RedisDistributedReaderWriterLockHandle"/> �������ͷ�����ʧ��ʱΪ��</returns>
        public RedisDistributedReaderWriterLockHandle? TryAcquireReadLock(TimeSpan timeout = default, CancellationToken cancellationToken = default) =>
            DistributedLockHelpers.TryAcquire(this, timeout, cancellationToken, isWrite: false);

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
        /// <returns>һ��<see cref="RedisDistributedReaderWriterLockHandle"/>���������ͷ���</returns>
        public RedisDistributedReaderWriterLockHandle AcquireReadLock(TimeSpan? timeout = null, CancellationToken cancellationToken = default) =>
            DistributedLockHelpers.Acquire(this, timeout, cancellationToken, isWrite: false);

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
        /// <returns>һ�� <see cref="RedisDistributedReaderWriterLockHandle"/> �������ͷ�����ʧ��ʱΪ��</returns>
        public ValueTask<RedisDistributedReaderWriterLockHandle?> TryAcquireReadLockAsync(TimeSpan timeout = default, CancellationToken cancellationToken = default) =>
            this.As<IInternalDistributedReaderWriterLock<RedisDistributedReaderWriterLockHandle>>().InternalTryAcquireAsync(timeout, cancellationToken, isWrite: false);

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
        /// <returns>һ��<see cref="RedisDistributedReaderWriterLockHandle"/>���������ͷ���</returns>
        public ValueTask<RedisDistributedReaderWriterLockHandle> AcquireReadLockAsync(TimeSpan? timeout = null, CancellationToken cancellationToken = default) =>
            DistributedLockHelpers.AcquireAsync(this, timeout, cancellationToken, isWrite: false);

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
        /// <returns>һ�� <see cref="RedisDistributedReaderWriterLockHandle"/> �������ͷ�����ʧ��ʱΪ��</returns>
        public RedisDistributedReaderWriterLockHandle? TryAcquireWriteLock(TimeSpan timeout = default, CancellationToken cancellationToken = default) =>
            DistributedLockHelpers.TryAcquire(this, timeout, cancellationToken, isWrite: true);

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
        /// <returns>һ��<see cref="RedisDistributedReaderWriterLockHandle"/>���������ͷ���</returns>
        public RedisDistributedReaderWriterLockHandle AcquireWriteLock(TimeSpan? timeout = null, CancellationToken cancellationToken = default) =>
            DistributedLockHelpers.Acquire(this, timeout, cancellationToken, isWrite: true);

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
        /// <returns>һ�� <see cref="RedisDistributedReaderWriterLockHandle"/> �������ͷ�����ʧ��ʱΪ��</returns>
        public ValueTask<RedisDistributedReaderWriterLockHandle?> TryAcquireWriteLockAsync(TimeSpan timeout = default, CancellationToken cancellationToken = default) =>
            this.As<IInternalDistributedReaderWriterLock<RedisDistributedReaderWriterLockHandle>>().InternalTryAcquireAsync(timeout, cancellationToken, isWrite: true);

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
        /// <returns>һ��<see cref="RedisDistributedReaderWriterLockHandle"/>���������ͷ���</returns>
        public ValueTask<RedisDistributedReaderWriterLockHandle> AcquireWriteLockAsync(TimeSpan? timeout = null, CancellationToken cancellationToken = default) =>
            DistributedLockHelpers.AcquireAsync(this, timeout, cancellationToken, isWrite: true);

    }
}