using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wootopia
{
	[RequireComponent(typeof(Image))]
	public class SequenceImagePlayer : MonoBehaviour
	{
		public enum Status
		{
			StartPlay,
			Playing,
			StopPlay,
			Stopped,
		}
		private Status status = Status.Stopped;
		[Header("COMPONENTS")]
		public Image Image;

		[Header("PROPERTIES")]
		private float timer;
		public bool isPlaying
		{
			get
			{
				return status == Status.Playing;
			}
			private set
			{
				
			}
		}
		private bool isStopped;
		public List<Sprite> Sprites = new List<Sprite>();
		public float PlaybackInterval;
		public bool BackAndForth = true;


		private void Start() { }

		private void Update()
		{
			if (!isPlaying)
			{
				return;
			}
			int imageCount = BackAndForth ? Sprites.Count * 2 - 1 : Sprites.Count;
			timer += Time.deltaTime;
			float currentTime = timer % (imageCount * PlaybackInterval);
			int currentIndex = Mathf.CeilToInt(currentTime / PlaybackInterval);
			currentIndex = currentIndex >= Sprites.Count ? imageCount - currentIndex : currentIndex;
			SetImage(currentIndex);

		}
		private void SetImage(int index)
		{
			if (!Image || Sprites.Count == 0)
			{
				return;
			}
			Image.sprite = Sprites[index];
		}

		public void Play()
		{
			
			if (!isPlaying)
			{
				timer = 0;
			}
			status = Status.Playing;
		}

		public void Stop()
		{
			status = Status.Stopped;
			SetImage(0);
		}
	}
}

