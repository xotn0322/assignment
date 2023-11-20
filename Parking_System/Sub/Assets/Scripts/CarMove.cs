using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{
    private int selectedNumber;
    private bool destroyed;
    //private float stopDuration = 0f; // 정지 시간
    private float moveSpeed = 2f; // 이동 속도
    public Texture2D[] carLicense;
    public Texture2D selectedLicense;
    SheetManagement sheetManagement;

    private void Start()
    {
        sheetManagement = GameObject.Find("Gate_In").GetComponent<SheetManagement>();

        //전체 번호판 중에서 랜덤으로 하나를 가져온다.
        if (carLicense.Length > 0)
        {
            int randomIndex = Random.Range(0, carLicense.Length);
            selectedLicense = carLicense[randomIndex];
        }

        destroyed = false;
        SelectNumber();
        StartCoroutine(MoveLeftAndStop());
    }

    private void Update()
    {
        if (destroyed)
        {
            destroyed = false;
            SelectNumber();
            StartCoroutine(MoveLeftAndStop());
        }   
    }

    private void SelectNumber()
    {
        int[] availableNumbers = ObjectMovementData.availableNumbers;

        // 사용 가능한 숫자 중에서 랜덤하게 선택
        selectedNumber = availableNumbers[Random.Range(0, availableNumbers.Length)];

        // 선택한 숫자를 사용했으므로 사용 불가능하도록 배열에서 제거
        ObjectMovementData.availableNumbers = RemoveNumberFromArray(availableNumbers, selectedNumber);
    }

    private int[] RemoveNumberFromArray(int[] array, int number)
    {
        // 배열에서 특정 숫자를 제외한 새로운 배열 생성
        int[] newArray = new int[array.Length - 1];
        int newIndex = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != number)
            {
                newArray[newIndex] = array[i];
                newIndex++;
            }
        }

        return newArray;
    }
    private IEnumerator MoveLeftAndStop()
    {
        float moveDistance = 2f;
        float stopDuration = 3f;

        // 왼쪽으로 이동
        Vector2 targetPosition = new Vector2(transform.position.x - moveDistance, transform.position.y);
        float distance = Mathf.Abs(targetPosition.x - transform.position.x);
        float duration = distance / 0.5f;

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector2.Lerp(transform.position, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 이동 후 정지 시간
        yield return new WaitForSeconds(stopDuration);
        /*
        if (sheetManagement.result == "false")
        {
            Destroy(gameObject);
        }
        */
        // MoveLeft 함수 호출
        MoveLeft();
    }

    private void MoveLeft()
    {
        float moveDistance = selectedNumber;
        StartCoroutine(MoveSmoothly(new Vector2(transform.position.x - moveDistance, transform.position.y)));
    }

    private IEnumerator MoveSmoothly(Vector2 targetPosition)
    {
        Vector2 startPosition = transform.position;
        float distance = Mathf.Abs(targetPosition.x - startPosition.x);
        float duration = distance / moveSpeed;

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector2.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 왼쪽 이동 후 회전
        transform.rotation = Quaternion.Euler(0f, 0f, -90f); // 왼쪽으로 이동 후 90도 회전

        // 위로 이동
        StartCoroutine(MoveUpSmoothly(new Vector2(transform.position.x, transform.position.y + 4.5f)));
    }

    private IEnumerator MoveUpSmoothly(Vector2 targetPosition)
    {
        Vector2 startPosition = transform.position;
        float distance = Mathf.Abs(targetPosition.y - startPosition.y);
        float duration = distance / moveSpeed;

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector2.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 이동 완료 후 정지
        StartCoroutine(StopMoving());
    }

    private IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(Random.Range(15f, 35f)); // 랜덤한 시간 동안 정지

        // 아래로 이동
        StartCoroutine(MoveDownSmoothly(new Vector2(transform.position.x, transform.position.y - 2.5f)));
    }

    private IEnumerator MoveDownSmoothly(Vector2 targetPosition)
    {
        Vector2 startPosition = transform.position;
        float distance = Vector2.Distance(startPosition, targetPosition);
        float duration = distance / moveSpeed;

        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector2.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, 0f); // 왼쪽으로 이동 후 90도 회전

        // 왼쪽으로 이동
        float moveDistance = 100f; // 왼쪽으로 이동할 거리 (임의의 값)
        StartCoroutine(MoveSmoothly(new Vector2(transform.position.x - moveDistance, transform.position.y)));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Destroy"))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        ObjectMovementData.availableNumbers = AddNumberToArray(ObjectMovementData.availableNumbers, selectedNumber);
        destroyed = true;
    }

    private int[] AddNumberToArray(int[] array, int number)
    {
        // 배열에 특정 숫자를 추가한 새로운 배열 생성
        int[] newArray = new int[array.Length + 1];

        for (int i = 0; i < array.Length; i++)
        {
            newArray[i] = array[i];
        }

        newArray[array.Length] = number;

        return newArray;
    }
}

