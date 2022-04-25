// �Զ�����
namespace Common.RedisLock.Core
{
    /// <summary>
    /// �䵱�ض����͵� <see cref="IDistributedSemaphore"/> ʵ���Ĺ����� ������������
    /// ������ע�볡���б� <see cref="IDistributedSemaphore"/> ������ʹ�á�
    /// </summary>
    public interface IDistributedSemaphoreProvider
    {
        /// <summary>
        /// ʹ�ø����� <paramref name="name"/> ����һ�� <see cref="IDistributedSemaphore"/> ʵ����
        /// </summary>
        IDistributedSemaphore CreateSemaphore(string name, int maxCount);
    }
}