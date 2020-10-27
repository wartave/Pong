using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bola : MonoBehaviour
{
    public float velocidad = 30.0f;

    //Contadores de goles
    public int golesIzquierda = 0;
    public int golesDerecha = 0;
    //Cajas de texto de los contadores
    public Text contadorIzquierda;
    public Text contadorDerecha;
    AudioSource fuenteDeAudio;
    public AudioClip audioGol, audioRaqueta, audioRebote, audioInicio, audioScreamer;

    public GameObject screamer;
    public bool scream=false;

    //Se ejecuta al arrancar
    void Start()
    {
        
       
        fuenteDeAudio = GetComponent<AudioSource>();
        //Pongo los contadores a 0
        contadorIzquierda.text = golesIzquierda.ToString();
        contadorDerecha.text = golesDerecha.ToString();

    }
    //Se ejecuta si choco con la raqueta
    void OnCollisionEnter2D(Collision2D micolision)
    {
        //Si me choco con la raqueta izquierda
        if (micolision.gameObject.name == "RaquetaIzquierda")
        {
            //Valor de x
            int x = 1;
            //Valor de y
            int y = direccionY(transform.position,
            micolision.transform.position);
            //Vector de dirección
            Vector2 direccion = new Vector2(x, y);
            //Aplico la velocidad a la bola
            GetComponent<Rigidbody2D>().velocity = direccion * velocidad;
            fuenteDeAudio.clip = audioRaqueta;
            fuenteDeAudio.Play();

        }
        //Si me choco con la raqueta derecha
        else if (micolision.gameObject.name == "RaquetaDerecha")
        {
            //Valor de x
            int x = -1;
            //Valor de y
            int y = direccionY(transform.position,
            micolision.transform.position);
            //Vector de dirección
            Vector2 direccion = new Vector2(x, y);
            //Aplico la velocidad a la bola
            GetComponent<Rigidbody2D>().velocity = direccion * velocidad;
            fuenteDeAudio.clip = audioRaqueta;
            fuenteDeAudio.Play();

        }
        //Para el sonido del rebote
        if (micolision.gameObject.name == "Arriba" ||
        micolision.gameObject.name == "Abajo")
        {
            //Reproduzco el sonido del rebote
            fuenteDeAudio.clip = audioRebote;
            fuenteDeAudio.Play();
        }

    }
    //Calculo la dirección de Y
    int direccionY(Vector2 posicionBola, Vector2 posicionRaqueta)
    {
        if (posicionBola.y > posicionRaqueta.y)
        {
            return 1;
        }
        else if (posicionBola.y < posicionRaqueta.y)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    //Reinicio la posición de la bola
    public void reiniciarBola(string direccion)
    {
        //Posición 0 de la bola
        transform.position = Vector2.zero;
        //Vector2.zero es lo mismo que new Vector2(0,0);
        //Velocidad inicial de la bola
        velocidad = 30;
        //Velocidad y dirección
        if (direccion == "Derecha")
        {
            //Incremento goles al de la derecha
            golesDerecha++;
            //Lo escribo en el marcador
            contadorDerecha.text = golesDerecha.ToString();
            //Reinicio la bola
            GetComponent<Rigidbody2D>().velocity = Vector2.right *
            velocidad;
            //Vector2.right es lo mismo que new Vector2(1,0)
        }
        else if (direccion == "Izquierda")
        {
            //Incremento goles al de la izquierda
            golesIzquierda++;
            //Lo escribo en el marcador
            contadorIzquierda.text = golesIzquierda.ToString();
            //Reinicio la bola
            GetComponent<Rigidbody2D>().velocity = Vector2.left *
            velocidad;
            //Vector2.right es lo mismo que new Vector2(-1,0)
        }

        if (golesIzquierda != 0 ||golesDerecha != 0)
        {
            velocidad = velocidad + ((golesDerecha + golesIzquierda) / 2f);
        }
        if (Random.Range(0, 10) != 2)
        {
            if (GameGlobal.GameStatus != "gol")
            {
                GameGlobal.GameStatus = "gol";
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else
        {
            GameGlobal.GameStatus = "screamer";
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

         
    }
    void Update()
    {
        if (fuenteDeAudio.clip == audioInicio)
        {
            fuenteDeAudio.volume = 0.10f;
        }
        else if(fuenteDeAudio.volume!=1f)
        {
            fuenteDeAudio.volume = 1f;
        }
        //Incremento la velocidad de la bola

        if (GameGlobal.GameStatus == "inicio")
        {
            fuenteDeAudio.clip = audioInicio;
            if (fuenteDeAudio.isPlaying != true)
            {
                fuenteDeAudio.Play();
            }
            
            Debug.Log(fuenteDeAudio.clip.length);
            Debug.Log(fuenteDeAudio.time);
            if (fuenteDeAudio.time >= fuenteDeAudio.clip.length-1)
            {
                Debug.Log("Hola");
                GameGlobal.GameStatus = "jugando";
            }

        }

        if (GameGlobal.GameStatus == "gol")
        {
            fuenteDeAudio.clip = audioGol;
            if (fuenteDeAudio.isPlaying != true)
            {
                fuenteDeAudio.Play();
            }

            Debug.Log("lenght:"+fuenteDeAudio.clip.length+" time:"+ fuenteDeAudio.time);

            if (fuenteDeAudio.time >= fuenteDeAudio.clip.length-.4f)
            {
                Debug.Log("Hola");
                GameGlobal.GameStatus = "jugando";

            }
        }

        if (GameGlobal.GameStatus == "screamer")
        {
            fuenteDeAudio.clip = audioScreamer;
            if (fuenteDeAudio.isPlaying != true)
            {
                fuenteDeAudio.Play();
                screamer.SetActive(true);
            }

            if (fuenteDeAudio.time >= fuenteDeAudio.clip.length - 1)
            {
                Debug.Log("Hola ghost");
                GameGlobal.GameStatus = "jugando";
                screamer.SetActive(false);
                scream = false;

            }
        }


        if (GameGlobal.GameStatus == "jugando")
        {
            if (GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Dynamic)
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                GetComponent<Rigidbody2D>().velocity = Vector2.right * velocidad;
            }
        }
        //Velocidad inicial hacia la derecha
        


        

    }

}
