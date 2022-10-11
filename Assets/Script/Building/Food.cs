using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : Job
{
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.GM();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Eat()
    {
        if (_gameManager.GetFood() > 0)
            _gameManager.Eat();
        else
        {
            
        }
        
        _gameManager.SetText();
    }

    public override void Produce()
    {
        _gameManager.AddFood();
        _gameManager.SetText();
        
    }
}
