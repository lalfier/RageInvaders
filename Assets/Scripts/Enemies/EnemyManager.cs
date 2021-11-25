using Zenject;
using UnityEngine;
using System.Collections.Generic;
using System;

public class EnemyManager : ITickable
{
    readonly Settings _settings;
    readonly Enemy.Factory _enemyFactory;
    readonly SignalBus _signalBus;
    readonly EnemyRegistry _enemyReg;
    readonly LevelBounds _bounds;

    Transform _enemiesParent;
    bool _gameStarted;
    int _direction;
    int _waveDifficulty;
    float _lastFireTime;

    public EnemyManager(
        Enemy.Factory enemyFactory, SignalBus signalBus,
        EnemyRegistry enemyReg, LevelBounds bounds, Settings settings)
    {
        _enemyFactory = enemyFactory;
        _signalBus = signalBus;
        _enemyReg = enemyReg;
        _bounds = bounds;
        _settings = settings;
    }

    public void Tick()
    {
        if (!_gameStarted)
        {
            return;
        }

        if(_enemyReg.EnemyCount == 0)
        {
            GenerateWave();
        }
        else
        {
            MoveHorizontal();
            Attack();
        }
    }

    public void Stop()
    {
        _gameStarted = false;
    }

    public void Start()
    {
        _gameStarted = true;
        _waveDifficulty = -1;
        GenerateWave();
        _direction = 1;
    }

    public void Exit()
    {
        Stop();
        while (_enemyReg.EnemyCount > 0)
        {
            _enemyReg.Enemies[_enemyReg.EnemyCount - 1].Despawn();
        }
    }

    private void GenerateWave()
    {
        int enemyCount = _settings.enemyColumns * _settings.enemyRows;
        for (int i = 0; i < enemyCount; i++)
        {
            _enemyFactory.Create();
        }

        List<Enemy> randList = new List<Enemy>(_enemyReg.Enemies);
        _enemiesParent = _enemyReg.Enemies[0].transform.parent;
        _enemiesParent.position = Vector3.zero;

        for (int posIndex = 0; posIndex < enemyCount; posIndex++)
        {
            float XOffset = (posIndex / _settings.enemyRows) * _settings.xOffset;   // Divide by rows
            float YOffset = (posIndex % _settings.enemyRows) * _settings.yOffset;   // Modulo gives remainder

            Vector3 startPos = new Vector3(-(float)(_settings.enemyColumns - 1) / 2 * _settings.xOffset, 0, _bounds.Bottom + _settings.topOffset);

            int randIndex = UnityEngine.Random.Range(0, randList.Count);
            randList[randIndex].gameObject.transform.position = startPos + new Vector3(XOffset, 0, YOffset);
            randList.RemoveAt(randIndex);
        }

        _waveDifficulty++;
        _signalBus.Fire<WaveCreatedSignal>();
    }

    private void MoveHorizontal()
    {
        float speedPerWave = _waveDifficulty * _settings.difficultyPerWave;
        _enemiesParent.transform.position += _direction * _enemiesParent.transform.right * (_settings.moveSpeed + speedPerWave) * Time.deltaTime;

        foreach (Enemy enemy in _enemyReg.Enemies)
        {
            if(_direction == 1 && ((enemy.transform.position.x + _settings.xOffset/_settings.enemyColumns) >= _bounds.Right))
            {
                MoveVertical();
            }
            if (_direction == -1 && ((enemy.transform.position.x - _settings.xOffset/_settings.enemyColumns) <= _bounds.Left))
            {
                MoveVertical();
            }
        }
    }

    private void MoveVertical()
    {
        _direction *= -1;
        Vector3 pos = _enemiesParent.position;
        pos.z += _settings.yOffset / 2;
        _enemiesParent.position = pos;
    }

    private void Attack()
    {
        if (Time.realtimeSinceStartup - _lastFireTime > _settings.maxShootInterval)
        {
            _lastFireTime = Time.realtimeSinceStartup;

            foreach (Enemy enemy in _enemyReg.Enemies)
            {
                if (UnityEngine.Random.value < (1.0f/(_enemyReg.EnemyCount+0.5f)))
                {
                    enemy.Fire();
                    break;
                }
            }
        }
    }

    [Serializable]
    public class Settings
    {
        public int enemyColumns;
        public int enemyRows;
        public float xOffset;
        public float yOffset;
        public float topOffset;
        public float moveSpeed;
        public float difficultyPerWave;
        public float maxShootInterval;
    }
}
