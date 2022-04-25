// �Զ�����
namespace Common.RedisLock.Core
{
    /// <summary>
    /// �䵱�ض����͵� <see cref="IDistributedLock"/> ʵ���Ĺ����� ������������
    /// ������ע�볡���б� <see cref="IDistributedLock"/> ������ʹ�á�
    /// </summary>
    public interface IDistributedLockProvider
    {
        /// <summary>
        /// ʹ�ø����� <paramref name="name"/> ����һ�� <see cref="IDistributedLock"/> ʵ����
        /// </summary>
        IDistributedLock CreateLock(string name);
    }
}