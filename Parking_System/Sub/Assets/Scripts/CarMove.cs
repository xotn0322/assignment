using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMove : MonoBehaviour
{
    private int selectedNumber;
    private bool destroyed;
    //private float stopDuration = 0f; // ���� �ð�
    private float moveSpeed = 2f; // �̵� �ӵ�
    public Texture2D[] carLicense;
    public Texture2D selectedLicense;
    SheetManagement sheetManagement;

    private void Start()
    {
        sheetManagement = GameObject.Find("Gate_In").GetComponent<SheetManagement>();

        //��ü ��ȣ�� �߿��� �������� �ϳ��� �����´�.
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

        // ��� ������ ���� �߿��� �����ϰ� ����
        selectedNumber = availableNumbers[Random.Range(0, availableNumbers.Length)];

        // ������ ���ڸ� ��������Ƿ� ��� �Ұ����ϵ��� �迭���� ����
        ObjectMovementData.availableNumbers = RemoveNumberFromArray(availableNumbers, selectedNumber);
    }

    private int[] RemoveNumberFromArray(int[] array, int number)
    {
        // �迭���� Ư�� ���ڸ� ������ ���ο� �迭 ����
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

        // �������� �̵�
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

        // �̵� �� ���� �ð�
        yield return new WaitForSeconds(stopDuration);
        /*
        if (sheetManagement.result == "false")
        {
            Destroy(gameObject);
        }
        */
        // MoveLeft �Լ� ȣ��
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

        // ���� �̵� �� ȸ��
        transform.rotation = Quaternion.Euler(0f, 0f, -90f); // �������� �̵� �� 90�� ȸ��

        // ���� �̵�
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

        // �̵� �Ϸ� �� ����
        StartCoroutine(StopMoving());
    }

    private IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(Random.Range(15f, 35f)); // ������ �ð� ���� ����

        // �Ʒ��� �̵�
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

        transform.rotation = Quaternion.Euler(0f, 0f, 0f); // �������� �̵� �� 90�� ȸ��

        // �������� �̵�
        float moveDistance = 100f; // �������� �̵��� �Ÿ� (������ ��)
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
        // �迭�� Ư�� ���ڸ� �߰��� ���ο� �迭 ����
        int[] newArray = new int[array.Length + 1];

        for (int i = 0; i < array.Length; i++)
        {
            newArray[i] = array[i];
        }

        newArray[array.Length] = number;

        return newArray;
    }
}

