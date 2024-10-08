using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
    private GameManager gameManager;
    private ScoreManager scoreManager;
    private BallCountManager ballCountManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        ballCountManager = FindObjectOfType<BallCountManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.CompareTag("Ball"))
        {
            HandleBallCollision(other);
        }
        else if (gameObject.CompareTag("Obstacle"))
        {
            if (other.gameObject.CompareTag("Player"))
            {
                gameManager.HandleGameOver();
            }
            else
            {
                HandleObstacleCollision(other);
            }
        }
    }

    void HandleBallCollision(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            BallMoving ballController = GetComponent<BallMoving>();
            if (ballController != null)
            {
                ballController.ReverseXVelocity();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void HandleObstacleCollision(Collider2D other)
    {
        GameObject obstacle = ObstacleWrapperManager.GetObstacle(gameObject);
        Renderer obstacleRenderer = obstacle.GetComponent<Renderer>();
        Renderer ballRenderer = other.GetComponent<Renderer>();

        if (obstacleRenderer != null && ballRenderer != null)
        {
            Color obstacleColor = obstacleRenderer.material.color;
            Color ballColor = ballRenderer.material.color;

            if (ColorManager.PrimaryColorsSet.Contains(obstacleColor))
            {
                HandlePrimaryColorCollision(obstacleColor, ballColor);
            }
            else
            {
                HandleBlendedColorCollision(obstacleColor, ballColor);
            }
        }
    }

    void HandlePrimaryColorCollision(Color obstacleColor, Color ballColor)
    {
        if (obstacleColor == ballColor)
        {
            ballCountManager.ModifyBallCount(ballCountManager.rewardBall);
            scoreManager.ModifyScore(scoreManager.rewardScore);
        }
        else
        {
            ballCountManager.ModifyBallCount(ballCountManager.penaltyBall);
        }
        Destroy(gameObject);
    }

    void HandleBlendedColorCollision(Color obstacleColor, Color ballColor)
    {
        if (ColorManager.IsBallColorOneOfBlended(obstacleColor, ballColor))
        {
            ChangeBlendedObstacleColor(obstacleColor, ballColor);
            scoreManager.ModifyScore(scoreManager.rewardScore);
        }
        else
        {
            ballCountManager.ModifyBallCount(ballCountManager.penaltyBall);
            Destroy(gameObject);
        }
    }

    void ChangeBlendedObstacleColor(Color obstacleColor, Color ballColor)
    {
        Color newObstacleColor = ColorManager.GetNewObstacleColor(obstacleColor, ballColor);

        GameObject obstacle = ObstacleWrapperManager.GetObstacle(gameObject);
        obstacle.gameObject.GetComponent<Renderer>().material.color = newObstacleColor;
    }
}
