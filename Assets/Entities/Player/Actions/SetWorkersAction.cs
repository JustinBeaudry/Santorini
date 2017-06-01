using UnityEngine;
using UnityEngine.UI;

public class SetWorkersAction : MonoBehaviour
{

  GameObject Worker;
  Material BaseWorkerMaterial;

  private void Awake()
  {
    Worker = Resources.Load("Worker") as GameObject;
    BaseWorkerMaterial = Resources.Load("BaseWorkerMaterial") as Material;
  }

  public void Do(Tile tile, GameAction gameAction)
  {
    if (tile != null && !tile.HasWorker())
    {

      GameObject worker = Instantiate(Worker, tile.transform);
      // Set worker color
      Material workerMaterial = new Material(BaseWorkerMaterial);
      workerMaterial.color = gameAction.Player.WorkerColor;
      worker.GetComponent<MeshRenderer>().material = workerMaterial;

      // Set worker position
      worker.transform.localPosition = new Vector3(0, 0.25f, 0);

      // Get Worker Component
      Worker _worker = worker.GetComponent<Worker>();

      // Set reference to workers tile
      _worker.CurrentTile = tile;

      // Add worker to player
      gameAction.Player.Workers.Add(_worker);
    }
    if (gameAction.Player.Workers.Count == gameAction.Player.MaxWorkers)
    {
      GameActionController.GameActionComplete();
    }
  }
}