using Cysharp.Threading.Tasks;
using PrimeTween;
using System.Threading;

public static class PrimeTweenUniTaskExtensions
{
    public static UniTask ToUniTask(this Tween tween, CancellationToken cancellationToken = default)
    {
        // Если твин уже невалиден, возвращаем завершенную задачу
        if (!tween.isAlive)
        {
            return UniTask.CompletedTask;
        }

        // Создаем Source для управления задачей. 
        // Использование типизированного источника предотвращает аллокации при ожидании.
        var utcs = new UniTaskCompletionSource();

        // Подписываемся на завершение твина
        tween.OnComplete(() =>
        {
            if (utcs.TrySetResult()) { }
        });

        // Обрабатываем CancellationToken
        if (cancellationToken.CanBeCanceled)
        {
            cancellationToken.Register(() =>
            {
                if (tween.isAlive)
                {
                    tween.Stop(); // Останавливаем твин при отмене токена
                }
                utcs.TrySetCanceled(cancellationToken);
            });
        }

        return utcs.Task;
    }
}