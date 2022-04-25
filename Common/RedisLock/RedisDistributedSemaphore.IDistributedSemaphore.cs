using Common.RedisLock.Core;
using Common.RedisLock.Core.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.RedisLock
{
    public partial class RedisDistributedSemaphore
    {
        // �Զ�����

        IDistributedSynchronizationHandle? IDistributedSemaphore.TryAcquire(TimeSpan timeout, CancellationToken cancellationToken) =>
            this.TryAcquire(timeout, cancellationToken);
        IDistributedSynchronizationHandle IDistributedSemaphore.Acquire(TimeSpan? timeout, CancellationToken cancellationToken) =>
            this.Acquire(timeout, cancellationToken);
        ValueTask<IDistributedSynchronizationHandle?> IDistributedSemaphore.TryAcquireAsync(TimeSpan timeout, CancellationToken cancellationToken) =>
            this.TryAcquireAsync(timeout, cancellationToken).Convert(To<IDistributedSynchronizationHandle?>.ValueTask);
        ValueTask<IDistributedSynchronizationHandle> IDistributedSemaphore.AcquireAsync(TimeSpan? timeout, CancellationToken cancellationToken) =>
            this.AcquireAsync(timeout, cancellationToken).Convert(To<IDistributedSynchronizationHandle>.ValueTask);

        /// <summary>
        /// ����ͬ����ȡ�ź���Ʊ֤�� �÷���
        /// <code>
        ///     using (var handle = mySemaphore.TryAcquire(...))
        ///     {
        ///         if (handle != null) { /* we have the ticket! */ }
        ///     }
        ///     // dispose releases the ticket if we took it
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ 0</param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ�� <see cref="RedisDistributedSemaphoreHandle"/> �������ͷ�Ʊ֤��ʧ��ʱΪ��</returns>
        public RedisDistributedSemaphoreHandle? TryAcquire(TimeSpan timeout = default, CancellationToken cancellationToken = default) =>
            DistributedLockHelpers.TryAcquire(this, timeout, cancellationToken);

        /// <summary>
        /// ͬ����ȡ�ź���Ʊ֤��������Գ�ʱ����ʧ�ܲ����� <see cref="TimeoutException"/>�� �÷���
        /// <code>
        ///     using (mySemaphore.Acquire(...))
        ///     {
        ///         /* we have the ticket! */
        ///     }
        ///     // dispose releases the ticket
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ <see cref="Timeout.InfiniteTimeSpan"/></param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ�� <see cref="RedisDistributedSemaphoreHandle"/> ���������ͷ�Ʊ֤</returns>
        public RedisDistributedSemaphoreHandle Acquire(TimeSpan? timeout = null, CancellationToken cancellationToken = default) =>
            DistributedLockHelpers.Acquire(this, timeout, cancellationToken);

        /// <summary>
        /// �����첽��ȡ�ź���Ʊ֤�� �÷���
        /// <code>
        ///     await using (var handle = await mySemaphore.TryAcquireAsync(...))
        ///     {
        ///         if (handle != null) { /* we have the ticket! */ }
        ///     }
        ///     // dispose releases the ticket if we took it
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ 0</param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ�� <see cref="RedisDistributedSemaphoreHandle"/> �������ͷ�Ʊ֤��ʧ��ʱΪ��</returns>
        public ValueTask<RedisDistributedSemaphoreHandle?> TryAcquireAsync(TimeSpan timeout = default, CancellationToken cancellationToken = default) =>
            this.As<IInternalDistributedSemaphore<RedisDistributedSemaphoreHandle>>().InternalTryAcquireAsync(timeout, cancellationToken);

        /// <summary>
        /// �첽��ȡ�ź���Ʊ֤��������Գ�ʱ����ʧ�ܲ����� <see cref="TimeoutException"/>�� �÷���
        /// <code>
        ///     await using (await mySemaphore.AcquireAsync(...))
        ///     {
        ///         /* we have the ticket! */
        ///     }
        ///     // dispose releases the ticket
        /// </code>
        /// </summary>
        /// <param name="timeout">�ڷ�����ȡ����֮ǰ�ȴ��೤ʱ�䡣 Ĭ��Ϊ <see cref="Timeout.InfiniteTimeSpan"/></param>
        /// <param name="cancellationToken">ָ������ȡ���ȴ�������</param>
        /// <returns>һ�� <see cref="RedisDistributedSemaphoreHandle"/> ���������ͷ�Ʊ֤</returns>
        public ValueTask<RedisDistributedSemaphoreHandle> AcquireAsync(TimeSpan? timeout = null, CancellationToken cancellationToken = default) =>
            DistributedLockHelpers.AcquireAsync(this, timeout, cancellationToken);
    }
}