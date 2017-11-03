using UnityEngine;
using System.Collections;

[System.Serializable]
public class AVLNode {

	public int key; 
	public int balance_factor; 
	public AVLNode left;
	public AVLNode right;

	public object abstractObject;

	public AVLNode()
	{
		key = -100;
		balance_factor = -100;
		left = right = null;
		abstractObject = null;
	}
	
	public void SRD(AVLNode p)
	{
		if (p != null) 
		{
			SRD (p.left);
			Debug.Log(p.key);
			SRD (p.right);
		}
	}
	
	public void SDR(AVLNode p)
	{
		if (p != null) 
		{
			SDR (p.left);
			SDR (p.right);
			Debug.Log(p.key);
		}
	}

	public void PrintTree(AVLNode root)
	{
		if (root != null) 
		{
			if (root.left != null && root.right != null) 
			{
				Debug.Log ("Node " + root.key + " has left child " + root.left.key + " and right child " + root.right.key); 
			} 
			else 
			{
				if (root.left != null) 
				{
					Debug.Log ("Node " + root.key + " has left child " + root.left.key + " and no right child"); 
				}
				else
				{
					if (root.right != null) 
					{
						Debug.Log ("Node " + root.key + " has right child " + root.right.key + " and no left child"); 
					}
				}
			}
		}
		if(root.left != null)
			PrintTree (root.left);
		if(root.right != null)
			PrintTree (root.right);
	}

	private int Max_length(AVLNode p, int max, int length)
	{ 
		int result = max;
		if (p != null)
		{ 
			result = Max_length(p.right, result, length + 1); 
			if ((p.left == null)&&(p.right == null)&&(max < length)) 
				result = length; 
			result = Max_length(p.left, result, length + 1); 
		} 
		return result;
	} 
	
	private void Compute_balance_factor(AVLNode p)
	{
		int hLeft,hRight; 
		hLeft = 1; 
		hRight = 1; 
		
		if(p.left != null) 
			hLeft = Max_length(p.left, hLeft, 1); 
		else 
			hLeft = 0; 
		if(p.right != null) 
			hRight = Max_length(p.right, hRight, 1); 
		else 
			hRight = 0; 
		p.balance_factor = hRight - hLeft; 
	} 
	
	private AVLNode Rotate_Right(AVLNode p)
	{
		AVLNode q = p.left;
		p.left = q.right;
		q.right = p;
		Compute_balance_factor (p);
		Compute_balance_factor (q);
		p = q;
		return p;
	}
	
	private AVLNode Rotate_Left(AVLNode p)
	{
		AVLNode q = p.right;
		p.right = q.left;
		q.left = p;
		Compute_balance_factor (q);
		Compute_balance_factor (p);
		p = q;
		return p;
	}
	
	private AVLNode Double_Rotate_Left(AVLNode p)
	{
		p.left = Rotate_Left (p.left);
		p = Rotate_Right (p);
		return p;
	}
	
	private AVLNode Double_Rotate_Right(AVLNode p)
	{
		p.right = Rotate_Right (p.right);
		p = Rotate_Left (p);
		return p;
	}
	
	public AVLNode Insert(AVLNode p, int x, object obj)
	{ 
		if (p == null || (p.key == -100) && (p.balance_factor == -100))
		{ // daca nodul curent este NULL atunci 
			p = new AVLNode(); //se aloca spatiu pentru el in zona heap 
			p.key = x; //informatia devine x 
			p.balance_factor = 0; // factorul de echilibru este 0 - nodul este echilibrat 
			p.right = null;// nodul se insereaza ca nod frunza 
			p.left = null; //deci referintele catre copii sai sunt NULL 
			p.abstractObject = obj;
			return p; 
		} 
		else 
		{ 
			if (x < p.key) //daca cheia care se doreste inserata este 
				//mai mica ca valoare decat informatia din nodul curent 
				p.left = Insert(p.left, x, obj);// atunci se insereaza 
			//in subarborele stang al nodului curent 
			else 
				if (x > p.key) //altfel daca cheia care se va insera 
					//e mai mare decat informatia din nodul curent 
					p.right = Insert(p.right, x, obj); // atunci se va insera 
			//in subarborele drept 
			else 
				Debug.Log("Nodul exista deja"); 
		} 
		p = echilibrare(p);// daca intervin cazuri de dezechilibru 
		//se va echilibra subarborele sau chiar arborele 
		return p;
	} 
	
	private AVLNode echilibrare(AVLNode p)
	{ 
		AVLNode w; 
		Compute_balance_factor(p);//se calculeaza factorul de echilibru a nodului curent p 
		if(p.balance_factor == -2)
		{// daca p nod este critic 
			w = p.left; // atunci w este copilul stanga al lui p 
			if (w.balance_factor == 1)// si daca acesta are factorul de echilibru 1 
				p = Double_Rotate_Right(p);// atunci se face dubla rotatie dreapta 
			else//altfel se face o simpla rotatie dreapta 
				p = Rotate_Right(p); 
		} 
		else 
			if(p.balance_factor == 2)
		{//daca p nod este critic 
			w = p.right;//w este copilul dreapta al nodului curent p 
			if (w.balance_factor == -1)// si acesta are factorul de ech -1 
				p = Double_Rotate_Left(p);//se face o dubla rotatie stanga 
			else//altfel se face o simpla rotatie stanga 
				p = Rotate_Left(p); 
		} 
		return p; 
	} 
	
	public AVLNode Delete(AVLNode p,int x)
	{ 
		AVLNode q,r,w; 
		if (p != null)// daca nodul curent este diferit de NULL 
			if (x < p.key) //cheia care se doreste stearsa este mai mica decat informatia din nod 
				p.left = Delete(p.left, x); // se cauta cheia de sters in subarborele stang al nodului curent 
		else 
			if (x > p.key) // daca cheia este mai mare 
				p.right = Delete(p.right, x);// se cauta in subarborele drept 
		else
		{ //daca cheia este egala cu informatia din nodul curent 
			Debug.Log("yess");
			q = p;//un nod q devine p 
			if (q.right == null) // daca copilul drept al lui q eate NULL 
				p = q.left;// atunci p devine q->stanga 
			else 
				if (q.left == null) //altfel daca copilul stang al lui q este NULL 
					p = q.right;// p devine q->dreapta 
			else
			{ 
				w = q.left;//altfel w este copilul stanga al lui q 
				r = q;// r devine q 
				if (w.right != null)// daca copilul drept al lui w nun este NULL 
				{ 
					while (w.right != null)
					{ 
						r = w; 
						w = w.right; 
					} 
					p.key = w.key; 
					q = w; 
					r.right = w.left; 
					r = p.left; 
					w = w.left; 
					if (r != null) 
						while ((r != w)&&(r != null))
					{ 
						r = echilibrare(r); 
						r = r.right; 
					} 
				} 
				else
				{ 
					p.key = w.key; 
					p.left = w.left; 
					q = w; 
				} 
			} 
			q = null;// se sterge q 
		} 
		if (p != null) 
			p = echilibrare (p);// se echilibreaza p daca nu este NULL 
		else
			p = new AVLNode ();
		return p; 
	} 

	public bool ContainsNode(AVLNode root, int nodeKey)
	{
		bool result = false;

		if (root != null) 
		{
			if (root.key == nodeKey)
				result = true;
			else
			{
				if(root.key < nodeKey)
					result = ContainsNode(root.right, nodeKey);
				else
					result = ContainsNode(root.left, nodeKey);
			}
		}
		return result;
	}

	
	public AVLNode GetHighestValueNood(AVLNode node)
	{
		AVLNode result = null;
		if (node != null) 
		{
			result = GetHighestValueNood(node.right);
		}
		else 
		{
			result = node;
		}
		return result;
	}

	public AVLNode GetLowestValueNood(AVLNode node)
	{
		AVLNode result = null;
		if (node.left != null) 
		{
			result = GetLowestValueNood (node.left);
		}
		else 
		{
			result = node;
		}
		return result;
	}
}
