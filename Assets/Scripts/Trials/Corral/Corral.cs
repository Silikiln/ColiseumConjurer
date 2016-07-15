using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
//A trial that requires the player to herd animals into specific areas
//objective count = number of animals that need to be herded
public class Corral : Trial {
    //init some stuff
    string[] animalTypes;
    GameObject animalPen;
    GameObject animal;
    GameObject[] pens;
    int corraledAnimals = 0;
    //constants
    string[] ALLTYPES = {
        "Cow",
        "Pig",
        "Chicken",
        "Sheep",
        "Rabbit"
    };

    //constructor
    public Corral()
    {
        Name = "Corral";
        Description = "Corral The Animals";

        BaseObjectiveCount = 1;

        //load any prefabs
        animalPen = LoadResource<GameObject>("AnimalPen");
        animal = LoadResource<GameObject>("Animal");
    }

    //setup trial function
    public override void Setup()
    {
        //setup the animal types based on object count
        int typeCount = 0;
        if (ObjectCount > ALLTYPES.Length)
            typeCount = ALLTYPES.Length;
        else{typeCount = ObjectCount;}
        animalTypes = new string[typeCount];
        for (int i = 0; i < animalTypes.Length; i++)
            animalTypes[i] = ALLTYPES[i];
        //spawn the animal pens
        float radius = 3;
        float delta = (2 * Mathf.PI) / animalTypes.Length;
        float angle = 0;
        Vector3 position;
        pens = new GameObject[animalTypes.Length];
        for (int i = 0; i < animalTypes.Length; i++, angle -= delta){
            position = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0);
            pens[i] = Instantiate(animalPen, position, Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg));
            pens[i].tag = animalTypes[i];
            pens[i].GetComponentInChildren<TextMesh>().text = animalTypes[i];
        }

        //call spawn animals to load the animals into the trial
        SpawnAnimals();


        base.Setup();
    }

    //do some things over some time
    void Update()
    {
        //check if the number of animals herded is equal to the requirements
        if(RequirementsMet)
            TrialHandler.Instance.TrialFinished();

        //check if the trial is currently being loaded in
        if (CurrentState == TrialState.Loading)
            return;
    }

    public void CorralSuccess(){
        corraledAnimals++;
    }

    //check if requirements have been met
    public override bool RequirementsMet
    {
        get{return corraledAnimals == ObjectiveCount;}
    }

    //randomly generate animals based on a provided integer
    public void SpawnAnimals(){SpawnAnimals(ObjectiveCount);}
    public void SpawnAnimals(int spawnCount)
    {
        Vector3 position;
        for (int i = 0; i < spawnCount; i++){
            position = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
            GameObject tempAnimal = Instantiate(animal, position, Quaternion.identity);
            tempAnimal.tag = animalTypes[Random.Range(0, animalTypes.Length)];
            tempAnimal.GetComponentInChildren<TextMesh>().text = tempAnimal.tag;
        }
    }

    public void modifyStatsByAnimal(GameObject tempAnimal){
        int size = 0;
        switch (tempAnimal.tag)
        {
            case "Cow":
                size = 20;
                break;
            case "Pig":
                size = 15;
                break;
            case "Chicken":
                size = 5;
                break;
            case "Sheep":
                size = 10;
                break;
            case "Rabbit":
                size = 1;
                break;
            default:
                break;
        }

        //tempAnimal.GetComponent<AnimalMover>().Size = size;
    }
}
