using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSlingshot : MonoBehaviour
{
    [Header("Configuración")]
    public float fuerzaMaxima = 15f;       // Qué tan fuerte puede ir
    public float distanciaMaxJalado = 3f;  // Hasta dónde puedes jalar
    public LineRenderer lineaJalado;       // La línea visual del jale

    private Rigidbody2D rb;
    private Vector2 posicionInicial;
    private Vector2 puntoJalado;
    private bool estaJalando = false;
    private bool yaLanzado = false;
    private Camera camara;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        camara = Camera.main;
        posicionInicial = transform.position;
        
        // Congela el personaje al inicio
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void Update()
    {
        if (yaLanzado) return; // Si ya se lanzó, no hacer nada

        // --- DETECTAR TOQUE / CLICK ---
        Vector2 inputPos = Vector2.zero;
        bool tocando = false;

        // Funciona en celular Y en PC para probar
        if (Input.touchCount > 0)
        {
            Touch toque = Input.GetTouch(0);
            inputPos = camara.ScreenToWorldPoint(toque.position);
            
            if (toque.phase == TouchPhase.Began || toque.phase == TouchPhase.Moved)
                tocando = true;
            else if (toque.phase == TouchPhase.Ended)
                Lanzar();
        }
        else if (Input.GetMouseButton(0))
        {
            inputPos = camara.ScreenToWorldPoint(Input.mousePosition);
            tocando = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Lanzar();
        }

        // --- LÓGICA DE JALADO ---
        if (tocando)
        {
            // Verifica que el toque esté cerca del personaje
            float distanciaAlPersonaje = Vector2.Distance(inputPos, posicionInicial);
            
            if (!estaJalando && distanciaAlPersonaje < 1.5f)
            {
                estaJalando = true; // Empezó a jalar
            }

            if (estaJalando)
            {
                // Limita hasta dónde puede jalarse
                Vector2 direccion = inputPos - posicionInicial;
                if (direccion.magnitude > distanciaMaxJalado)
                    direccion = direccion.normalized * distanciaMaxJalado;

                puntoJalado = posicionInicial + direccion;
                
                // Mueve visualmente el personaje
                transform.position = puntoJalado;

                // Dibuja la línea de jale
                DibujarLinea(true);
            }
        }
    }

    void Lanzar()
    {
        if (!estaJalando) return;

        estaJalando = false;
        yaLanzado = true;

        // Desbloquea la física
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 1;

        // La fuerza va en dirección OPUESTA al jalado
        Vector2 direccionLanzamiento = posicionInicial - (Vector2)transform.position;
        Vector2 fuerza = direccionLanzamiento.normalized * 
                         (direccionLanzamiento.magnitude / distanciaMaxJalado) * fuerzaMaxima;
        
        rb.AddForce(fuerza, ForceMode2D.Impulse);

        // Oculta la línea
        DibujarLinea(false);

        // Reinicia después de 3 segundos
        Invoke("Reiniciar", 3f);
    }

    void DibujarLinea(bool mostrar)
    {
        if (lineaJalado == null) return;
        
        lineaJalado.enabled = mostrar;
        if (mostrar)
        {
            lineaJalado.SetPosition(0, transform.position);
            lineaJalado.SetPosition(1, posicionInicial);
        }
    }

    void Reiniciar()
    {
        yaLanzado = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.position = posicionInicial;
    }
}