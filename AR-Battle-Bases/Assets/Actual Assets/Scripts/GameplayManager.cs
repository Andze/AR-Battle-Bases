using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    struct Monster
    {
        public string name { get; private set; }
        public int hp { get; set; }
        public Ability[] abilities { get; private set; }

        public Monster(string name, int hp, params Ability[] abilities)
        {
            this.name = name;
            this.hp = hp;
            this.abilities = abilities;
        }
    }

    struct Ability
    {
        public enum Type { Fire, Water, Earth, Normal };

        public string name { get; private set; }
        public Type type { get; private set; }
        public int damage { get; private set; }
        public int hitPercent { get; private set; }
        public int critChance { get; private set; }

        public Ability(string name, Type type, int damage, int hitPercent, int critChance)
        {
            this.name = name;
            this.type = type;
            this.damage = damage;
            this.hitPercent = hitPercent;
            this.critChance = critChance;
        }
    }

    class PlayableMonster
    {
        public Monster monster;
        public int id;
        public GameObject sceneObject;
        public bool isSpawned;

        public PlayableMonster(Monster monster, int id, GameObject sceneObject = null)
        {
            this.monster = monster;
            this.id = id;
            this.sceneObject = sceneObject;
            isSpawned = true;
        }
    }

    // --------------------------------------------------------------------------------------------------------------------------- //

    int currentTurnId = 0;
    bool gameOver = false;

    public GameObject[] monsterSceneObjects;

    PlayableMonster[] monsters =
    {
        new PlayableMonster(
            new Monster("Volcanic Bunny", 100,
            new Ability("Kick", Ability.Type.Normal, 10, 100, 33),
            new Ability("Headbutt", Ability.Type.Normal, 20, 66, 20),
            new Ability("Ember", Ability.Type.Fire, 15, 75, 33),
            new Ability("Flamethrower", Ability.Type.Fire, 50, 20, 20)),
            0),

        new PlayableMonster(
            new Monster("Water Bunny", 100,
            new Ability("Normal Ability 1", Ability.Type.Normal, 10, 100, 33),
            new Ability("Normal Ability 2", Ability.Type.Normal, 20, 66, 20),
            new Ability("Water Ability 1", Ability.Type.Fire, 15, 75, 33),
            new Ability("Water Ability 2", Ability.Type.Fire, 50, 20, 20)),
            1)
    };

    public void Start()
    {
        for (int i = 0; i < monsters.Length && i < monsterSceneObjects.Length; i++) {
            monsters[i].sceneObject = monsterSceneObjects[i];
            SetupSceneObject(monsters[i].sceneObject);
        }

        SpawnMonster(0);
        SpawnMonster(1);
    }

    public void Update()
    {
        
    }

    // --------------------------------------------------------------------------------------------------------------------------- //

    private PlayableMonster FindMonster(int id)
    {
        for (int i = 0; i < monsters.Length; i++) {
            if (monsters[i].id == id) {
                return monsters[i];
            }
        }

        return null;
    }

    private void SetupSceneObject(GameObject sceneObject)
    {
        // TODO: Implement
    }

    public void SpawnMonster(int id)
    {
        PlayableMonster m = FindMonster(id);

        if (m != null) {
            m.isSpawned = true;

            if (m.sceneObject != null) {
                m.sceneObject.GetComponent<BunnyBehaviour>().PlayAnimation(0);
            }
        }
    }

    public void PlayerMonsterAttack(int abilityIndex)
    {
        MonsterAttack(0, abilityIndex);
    }

    public void MonsterAttack(int id, int abilityIndex)
    {
        if (id != currentTurnId || gameOver == true) {
            return;
        }

        PlayableMonster m = FindMonster(id);

        if (m != null && m.isSpawned && m.monster.hp != 0) {
            PlayableMonster target = monsters[(id + 1) % monsters.Length];
            
            Ability usedAbility = m.monster.abilities[abilityIndex];
            bool attackSuccess = Random.Range(1, 100) <= usedAbility.hitPercent;
            m.sceneObject.GetComponent<BunnyBehaviour>().PlayAnimation(1);

            if (attackSuccess) {
                float damageMultiplier = Random.Range(1, 100) <= usedAbility.critChance ? 1.5f : 1.0f;
                target.monster.hp -= (int)(usedAbility.damage * damageMultiplier);
                target.sceneObject.GetComponent<BunnyBehaviour>().PlayAnimation(2, 0.75f);
                target.sceneObject.GetComponent<BunnyBehaviour>().healthBar.UpdateVisuals(target.monster.hp);

                if (target.monster.hp <= 0) {
                    target.monster.hp = 0;
                    MonsterKill(target.id);
                }
            }

            currentTurnId = target.id;

            if (currentTurnId != 0) {
                StartCoroutine(AttackAfterDelay(currentTurnId, 3.0f));
            }
        }
    }

    private IEnumerator AttackAfterDelay(int id, float delay)
    {
        yield return new WaitForSeconds(delay);
        MonsterAttack(id, Random.Range(0, monsters[id].monster.abilities.Length - 1));
        yield return null;
    }

    private void MonsterKill(int id)
    {
        StartCoroutine(monsters[id].sceneObject.GetComponent<BunnyBehaviour>().Die(2.0f));
        gameOver = true;
    }
}
