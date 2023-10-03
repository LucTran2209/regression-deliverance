using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float max_health = 500f;
    public float current_health { get; private set; }
    private Animator m_animator;
    private Rigidbody2D m_rigidbody;
    private Collider2D m_collider;
    // Start is called before the first frame update
    void Start()
    {
        current_health = max_health;
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<Collider2D>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    public void TakeDmg(float dmg)
    {
        current_health = Mathf.Clamp(current_health - dmg, 0, max_health);
        if (current_health <= 0) {
			//Death
			m_animator.SetTrigger("Death");
            m_rigidbody.gravityScale = 0;
            m_collider.enabled = false;
            StartCoroutine(DestroyAfterSeconds(2f));
		}
        else {
            //Hit
            m_animator.SetTrigger("Hit");
        }
    }

	IEnumerator DestroyAfterSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Destroy(gameObject);
	}
}
