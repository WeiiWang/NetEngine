using Common.RedisLock.Core;
using Common.RedisLock.Core.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.RedisLock
{
    public partial class RedisDistributedLock
    {
        // �Զ�����

        IDistributedSynchronizationHandle? IDistributedLock.TryAcquire(TimeSpan timeout, CancellationToken cancellationToken) =>
            this.TryAcquire(timeout, cancellationToken);
        IDistributedSynchronizationHandle IDistributedLock.Acquire(TimeSpan? timeout, CancellationToken cancellationToken) =>
            this.Acquire(timeout, cancellationToken);
        ValueTask<IDistributedSynchronizationHandle?> IDistributedLock.TryAcquireAsync(TimeSpan timeout, CancellationToken cancellationToken) =>
            this.TryAcquireAsync(timeout, cancellationToken).Convert(To<IDistributedSynchronizationHandle?>.ValueTask);
        ValueTask<IDistributedSynchronizationHandle> IDistributedLock.AcquireAsync(TimeSpan? timeout, CancellationToken cancellationToken) =>
            this.AcquireAsync(timeout, cancellationToken).Convert(To<IDistributedSynchronizationHandle>.ValueTask);

        /// <summary>
        /// ����ͬ����ȡ���� �÷���
        /// <code>
        ///     using (var handle = myLock.TryAcquire(...))
        ///     {
        ///         if (handle != null) { /* we have the lock! */ }
        ///     }
        ///     // dispose releases the lock if we took it
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ 0</param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ�� <see cref="RedisDistributedLockHandle"/> �������ͷ�������ʧ��ʱΪ��</returns>
        public RedisDistributedLockHandle? TryAcquire(TimeSpan timeout = default, CancellationToken cancellationToken = default) =>
            DistributedLockHelpers.TryAcquire(this, timeout, cancellationToken);

        /// <summary>
        /// ͬ����ȡ����������Գ�ʱ����ʧ�ܲ����� <see cref="TimeoutException"/>�� �÷���
        /// <code>
        ///     using (myLock.Acquire(...))
        ///     {
        ///         /* we have the lock! */
        ///     }
        ///     // dispose releases the lock
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ <see cref="Timeout.InfiniteTimeSpan"/></param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ��<see cref="RedisDistributedLockHandle"/>���������ͷ���</returns>
        public RedisDistributedLockHandle Acquire(TimeSpan? timeout = null, CancellationToken cancellationToken = default) =>
            DistributedLockHelpers.Acquire(this, timeout, cancellationToken);

        /// <summary>
        /// �����첽��ȡ���� �÷���
        /// <code>
        ///     await using (var handle = await myLock.TryAcquireAsync(...))
        ///     {
        ///         if (handle != null) { /* we have the lock! */ }
        ///     }
        ///     // dispose releases the lock if we took it
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ 0</param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ�� <see cref="RedisDistributedLockHandle"/> �������ͷ�������ʧ��ʱΪ��</returns>
        public ValueTask<RedisDistributedLockHandle?> TryAcquireAsync(TimeSpan timeout = default, CancellationToken cancellationToken = default) =>
            this.As<IInternalDistributedLock<RedisDistributedLockHandle>>().InternalTryAcquireAsync(timeout, cancellationToken);

        /// <summary>
        /// �첽��ȡ����������Գ�ʱ����ʧ�ܲ����� <see cref="TimeoutException"/>�� �÷���
        /// <code>
        ///     await using (await myLock.AcquireAsync(...))
        ///     {
        ///         /* we have the lock! */
        ///     }
        ///     // dispose releases the lock
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ <see cref="Timeout.InfiniteTimeSpan"/></param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ��<see cref="RedisDistributedLockHandle"/>���������ͷ���</returns>
        public ValueTask<RedisDistributedLockHandle> AcquireAsync(TimeSpan? timeout = null, CancellationToken cancellationToken = default) =>
            DistributedLockHelpers.AcquireAsync(this, timeout, cancellationToken);
    }
}