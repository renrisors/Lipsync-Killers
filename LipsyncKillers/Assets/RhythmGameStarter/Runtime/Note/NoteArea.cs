﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using System;

namespace RhythmGameStarter
{
    public class NoteArea : MonoBehaviour
    {
        [Title("Events")]
        [CollapsedEvent] public UnityEvent OnInteractionBegin;
        [CollapsedEvent] public UnityEvent OnInteractionEnd;

        [Title("Effect")]
        public NoteEffect sustainEffect;

        private TapEffectPool effect;

        private Track track;

        private Camera _camera;

        private SongManager songManager;

        void Start()
        {
            track = GetComponentInParent<Track>();
            effect = GetComponentInParent<TapEffectPool>();
            songManager = GetComponentInParent<SongManager>();
            _camera = Camera.main;
        }

        private List<Note> notesInRange = new List<Note>();

        private Note currentNote;

        private int currentFingerID = -1;

        private Vector2 touchDownPosition;

        private Vector3 touchDownNotePosition;

        private LongNoteDetecter longNoteDetecter;

        //For recording mode
        private float touchDownTime;
        private float touchDownTimeInSong;
        private bool hasTriggeredSomething;
        private bool hasPossibleDirection;
        private Note.SwipeDirection possibleDirection;


        [HideInInspector]
        public KeyboardInputHandler keyboardInputHandler;

        [HideInInspector]
        public string key;

        void Update()
        {
            if (keyboardInputHandler)
            {
                if (Input.GetKeyDown(key))
                {
                    var fakeTouch = new Touch();
                    fakeTouch.phase = TouchPhase.Began;
                    TriggerNote(fakeTouch);
                }
                else if (Input.GetKeyUp(key))
                {
                    if (currentNote != null)
                        currentNote.inInteraction = false;

                    OnInteractionEnd.Invoke();
                }
            }

            var touch = Input.touches.Where(x => x.fingerId == currentFingerID).FirstOrDefault();

            if (currentFingerID != -1)
            {
                if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                {
                    if (currentNote != null)
                        currentNote.inInteraction = false;

                    OnInteractionEnd.Invoke();
                }
            }

            //// Handling Recording mode
            if (NoteRecorder.INSTANCE && NoteRecorder.INSTANCE.IsRecording)
            {
                Note.NoteAction action = Note.NoteAction.Tap;
                Note.SwipeDirection direction = Note.SwipeDirection.Left;

                bool hasDirectionKey(KeyCode key)
                {
                    return Input.GetKey(key) || Input.GetKeyUp(key) || Input.GetKeyDown(key);
                }

                if (hasTriggeredSomething && !hasPossibleDirection)
                {
                    var diff = _camera.ScreenToWorldPoint(touchDownPosition) - _camera.ScreenToWorldPoint(touch.position);
                    if ((keyboardInputHandler && hasDirectionKey(keyboardInputHandler.left)) || diff.x >= NoteRecorder.INSTANCE.swipeThreshold)
                    {
                        possibleDirection = Note.SwipeDirection.Left;
                        hasPossibleDirection = true;
                    }
                    else if ((keyboardInputHandler && hasDirectionKey(keyboardInputHandler.right)) || diff.x <= -NoteRecorder.INSTANCE.swipeThreshold)
                    {
                        possibleDirection = Note.SwipeDirection.Right;
                        hasPossibleDirection = true;
                    }
                    else if ((keyboardInputHandler && hasDirectionKey(keyboardInputHandler.up)) || diff.y <= -NoteRecorder.INSTANCE.swipeThreshold)
                    {
                        possibleDirection = Note.SwipeDirection.Up;
                        hasPossibleDirection = true;
                    }
                    else if ((keyboardInputHandler && hasDirectionKey(keyboardInputHandler.down)) || diff.y >= NoteRecorder.INSTANCE.swipeThreshold)
                    {
                        possibleDirection = Note.SwipeDirection.Down;
                        hasPossibleDirection = true;
                    }

                    /*/Diagonais
                    else if ((keyboardInputHandler && (hasDirectionKey(keyboardInputHandler.up) && hasDirectionKey(keyboardInputHandler.left)) || ((diff.y <= -NoteRecorder.INSTANCE.swipeThreshold) && (diff.x >= NoteRecorder.INSTANCE.swipeThreshold))))
                    {
                        possibleDirection = Note.SwipeDirection.UpLeft;
                        hasPossibleDirection = true;
                    }
                    else if ((keyboardInputHandler && (hasDirectionKey(keyboardInputHandler.up) && hasDirectionKey(keyboardInputHandler.right)) || ((diff.y <= -NoteRecorder.INSTANCE.swipeThreshold) && (diff.x <= -NoteRecorder.INSTANCE.swipeThreshold))))
                    {
                        possibleDirection = Note.SwipeDirection.UpRight;
                        hasPossibleDirection = true;
                    }
                    else if ((keyboardInputHandler && (hasDirectionKey(keyboardInputHandler.down) && hasDirectionKey(keyboardInputHandler.left)) || ((diff.y >= NoteRecorder.INSTANCE.swipeThreshold) && (diff.x >= NoteRecorder.INSTANCE.swipeThreshold))))
                    {
                        possibleDirection = Note.SwipeDirection.DownLeft;
                        hasPossibleDirection = true;
                    }
                    else if ((keyboardInputHandler && (hasDirectionKey(keyboardInputHandler.down) && hasDirectionKey(keyboardInputHandler.left)) || ((diff.y >= NoteRecorder.INSTANCE.swipeThreshold) && (diff.x <= -NoteRecorder.INSTANCE.swipeThreshold))))
                    {
                        possibleDirection = Note.SwipeDirection.DownRight;
                        hasPossibleDirection = true;
                    }*/
                }

                if ((currentFingerID != -1 && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)) || (keyboardInputHandler && Input.GetKeyUp(key)))
                {
                    //This could be a tap, long press, or even swipe
                    var duration = songManager.songPosition - touchDownTimeInSong;

                    if (hasPossibleDirection)
                    {
                        action = Note.NoteAction.Swipe;
                        direction = possibleDirection;
                    }

                    //The duration is long enough
                    if (duration > NoteRecorder.INSTANCE.longNoteThershold)
                        action = Note.NoteAction.LongPress;

                    hasTriggeredSomething = false;
                    hasPossibleDirection = false;
                    currentFingerID = -1;

                    NoteRecorder.INSTANCE.RecordNote(new NoteRecorder.NoteEventData()
                    {
                        track = track,
                        action = action,
                        duration = duration,
                        startTime = touchDownTimeInSong,
                        swipeDirection = direction,
                    });
                }
                return;
            }

            if (currentNote || (currentNote && keyboardInputHandler))
            {
                switch (currentNote.action)
                {
                    case Note.NoteAction.LongPress:
                        if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary || (keyboardInputHandler && Input.GetKey(key)))
                        {
                            if (longNoteDetecter && longNoteDetecter.exitedLineArea)
                            {
                                // print("noteFinished");

                                AddCombo(currentNote, touchDownNotePosition);

                                longNoteDetecter.OnTouchUp();
                                longNoteDetecter = null;
                                currentNote = null;

                                sustainEffect.StopEffect();
                            }

                        }
                        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled || (keyboardInputHandler && Input.GetKeyUp(key)))
                        {
                            if (longNoteDetecter.exitedLineArea)
                            {
                                // print("noteFinished");
                                AddCombo(currentNote, touchDownNotePosition);
                            }
                            else
                            {
                                print("noteFailed");
                                songManager.comboSystem.BreakCombo();
                            }

                            longNoteDetecter.OnTouchUp();
                            longNoteDetecter = null;
                            currentNote = null;

                            sustainEffect.StopEffect();
                        }
                        break;
                    case Note.NoteAction.Swipe:
                        if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended || keyboardInputHandler)
                        {
                            if (currentNote.alreadyMissed)
                            {
                                currentNote = null;
                                return;
                            }

                            bool addCombo = false;
                            if (keyboardInputHandler)
                            {
                                switch (currentNote.swipeDirection)
                                {
                                    case Note.SwipeDirection.Up:
                                        addCombo = Input.GetKey(keyboardInputHandler.up);
                                        break;
                                    case Note.SwipeDirection.Down:
                                        addCombo = Input.GetKey(keyboardInputHandler.down);
                                        break;
                                    case Note.SwipeDirection.Left:
                                        addCombo = Input.GetKey(keyboardInputHandler.left);
                                        break;
                                    case Note.SwipeDirection.Right:
                                        addCombo = Input.GetKey(keyboardInputHandler.right);
                                        break;

                                        /*/Diagonais
                                        case Note.SwipeDirection.UpLeft:
                                            addCombo = Input.GetKey(keyboardInputHandler.upLeft);
                                            break;
                                        case Note.SwipeDirection.UpRight:
                                            addCombo = Input.GetKey(keyboardInputHandler.upRight);
                                            break;
                                        case Note.SwipeDirection.DownLeft:
                                            addCombo = Input.GetKey(keyboardInputHandler.downLeft);
                                            break;
                                        case Note.SwipeDirection.DownRight:
                                            addCombo = Input.GetKey(keyboardInputHandler.downRight);
                                            break;*/
                                }
                            }

                            if (!addCombo && currentFingerID != -1 && touch.phase != TouchPhase.Began)
                            {
                                var diff = _camera.ScreenToWorldPoint(touchDownPosition) - _camera.ScreenToWorldPoint(touch.position);
                                // var diff = touchDownPosition - touch.position;
                                switch (currentNote.swipeDirection)
                                {
                                    case Note.SwipeDirection.Up:
                                        addCombo = diff.y <= -currentNote.swipeThreshold;
                                        break;
                                    case Note.SwipeDirection.Down:
                                        addCombo = diff.y >= currentNote.swipeThreshold;
                                        break;
                                    case Note.SwipeDirection.Left:
                                        addCombo = diff.x >= currentNote.swipeThreshold;
                                        break;
                                    case Note.SwipeDirection.Right:
                                        addCombo = diff.x <= -currentNote.swipeThreshold;
                                        break;

                                    /*/Diagonais
                                    case Note.SwipeDirection.UpRight:
                                        addCombo = diff.y <= -currentNote.swipeThreshold;
                                        break;
                                    case Note.SwipeDirection.UpLeft:
                                        addCombo = diff.y <= -currentNote.swipeThreshold;
                                        break;
                                    case Note.SwipeDirection.DownRight:
                                        addCombo = diff.y >= currentNote.swipeThreshold;
                                        break;
                                    case Note.SwipeDirection.DownLeft:
                                        addCombo = diff.y >= currentNote.swipeThreshold;
                                        break;*/

                                }
                                // print(currentNote.swipeDirection + " " + diff.x + " " + addCombo + " " + touch.position + " " + touch.fingerId + " " + touch.phase);
                            }

                            if (addCombo)
                            {
                                PlayHitSound(currentNote);
                                AddCombo(currentNote, touchDownNotePosition);

                                if (effect && !currentNote.noTapEffect)
                                    effect.EmitEffects(currentNote.transform);

                                KillNote(currentNote);
                                currentNote = null;
                            }
                            else
                            {
                                print("noteFailed");
                            }
                        }
                        break;
                }
            }
        }

        public void ResetNoteArea()
        {
            currentFingerID = -1;
            notesInRange.Clear();
        }

        public void TriggerNote(Touch touch)
        {
            if (songManager.songPaused)
                return;

            currentFingerID = touch.fingerId;
            if (touch.phase == TouchPhase.Began)
            {
                OnInteractionBegin.Invoke();
            }

            var note = notesInRange.FirstOrDefault();

            //// Handling Recording mode
            if (NoteRecorder.INSTANCE && NoteRecorder.INSTANCE.IsRecording)
            {
                if (touch.phase != TouchPhase.Began) return;
                hasTriggeredSomething = true;
                touchDownPosition = touch.position;
                // touchDownTime = Time.time;
                touchDownTimeInSong = songManager.songPosition;
                return;
            }

            if (note)
            {
                switch (note.action)
                {
                    case Note.NoteAction.Tap:
                        if (touch.phase != TouchPhase.Began) break;

                        if (effect && !note.noTapEffect)
                            effect.EmitEffects(note.transform);

                        PlayHitSound(note);
                        AddCombo(note, note.transform.position);

                        KillNote(note);

                        break;
                    case Note.NoteAction.LongPress:
                        if (touch.phase != TouchPhase.Began) break;

                        if (effect && !note.noTapEffect)
                            effect.EmitEffects(note.transform);

                        currentNote = note;
                        note.inInteraction = true;

                        touchDownNotePosition = note.transform.position;

                        longNoteDetecter = note.GetNoteDetecter();
                        longNoteDetecter.OnTouchDown();

                        PlayHitSound(note);

                        notesInRange.Remove(note);

                        sustainEffect.StartEffect(null);

                        break;
                    case Note.NoteAction.Swipe:
                        if (touch.phase != TouchPhase.Began) break;

                        notesInRange.Remove(note);

                        touchDownPosition = touch.position;
                        currentNote = note;
                        note.inInteraction = true;

                        touchDownNotePosition = note.transform.position;
                        break;
                }
            }
        }

        private void PlayHitSound(Note note)
        {
            if (!note.noHitSound)
            {
                track.trackHitAudio.clip = note.hitSound;
                track.trackHitAudio.Play();
            }
        }

        private void AddCombo(Note note, Vector3 touchDownPosition)
        {
            var noteLocalPositionInTrack = note.transform.parent.parent.InverseTransformPoint(touchDownPosition);
            var diff = track.lineArea.localPosition.y - noteLocalPositionInTrack.y;
            songManager.comboSystem.AddCombo(1, Mathf.Abs(diff), note.score);
            songManager.trackManager.onNoteTriggered.Invoke(note);
        }

        private void KillNoteAnimation(Note note)
        {
            if (note.killAnim)
            {
                var anim = note.GetComponent<Animation>();
                anim.Play(note.killAnim.name, PlayMode.StopAll);
                note.transform.SetParent(null);

                if (songManager.trackManager.useNotePool)
                    StartCoroutine(DelayResetNote(note.gameObject, note.killAnim.length));
                else
                    Destroy(note.gameObject, note.killAnim.length);
            }
            else
            {
                if (songManager.trackManager.useNotePool)
                    songManager.trackManager.ResetNoteToPool(note.gameObject);
                else
                    Destroy(note.gameObject);
            }
        }

        IEnumerator DelayResetNote(GameObject note, float delay)
        {
            yield return new WaitForSeconds(delay);
            songManager.trackManager.ResetNoteToPool(note);
        }

        private void KillNote(Note note)
        {
            KillNoteAnimation(note);
            notesInRange.Remove(note);
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.tag == "Note")
            {
                notesInRange.Add(col.GetComponent<Note>());
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.tag == "Note")
            {
                var outOfRange = col.GetComponent<Note>();
                notesInRange.Remove(outOfRange);

                if (currentNote != null)
                    if (currentNote == outOfRange)
                    {
                        currentNote = null;
                    }
            }
        }
    }
}