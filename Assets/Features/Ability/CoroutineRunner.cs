using Game.Framework;
using System.Collections;
using System.Threading.Tasks;

public class CoroutineRunner : GameService<CoroutineRunner>
{
    private IEnumerator RunWrapper(IEnumerator routine, TaskCompletionSource<bool> tcs)
    {
        yield return routine;
        tcs.SetResult(true);
    }

    public Task Run(IEnumerator routine)
    {
        var tcs = new TaskCompletionSource<bool>();
        var rtn = RunWrapper(routine, tcs);

        StartCoroutine(rtn);

        return tcs.Task;
    }
}
