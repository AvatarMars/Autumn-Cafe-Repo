using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Volume))]
public class PostProcessVolumeTrigger : MonoBehaviour
{
    private Volume _volumeToTrigger;
    private Volume[] _allVolumes;
    private Dictionary<Volume, float> _originalVolumeWeights;

    public bool shouldExecute;

    private void Awake()
    {
        _volumeToTrigger = GetComponent<Volume>();
        _allVolumes = GetComponents<Volume>()
            .Where(volume => volume != _volumeToTrigger)
            .ToArray();

        _originalVolumeWeights = _allVolumes
            .ToDictionary(
                volume => volume,
                volume => volume.weight);
    }

    private void Update()
    {
        if (shouldExecute && _volumeToTrigger.weight <= .1f)
        {
            shouldExecute = false;
            ActivateVolume();
        }

        if (shouldExecute && _volumeToTrigger.weight >= .9f)
        {
            shouldExecute = false;
            DeactivateVolume();
        }
    }

    public void ActivateVolume()
    {
        StartCoroutine(ActivateVolumeToTrigger());
    }

    public void DeactivateVolume()
    {
        StartCoroutine(DeactivateVolumeToTrigger());
    }

    IEnumerator ActivateVolumeToTrigger()
    {
        var weight = 1f;
        while (weight >= 0)
        {
            foreach (var volume in _allVolumes)
            {
                if (weight <= volume.weight) volume.weight = weight;
                yield return new WaitForSeconds(.1f);
            }

            var newVolumeWeight = 1 - weight;
            if (newVolumeWeight >= _volumeToTrigger.weight) _volumeToTrigger.weight = newVolumeWeight;
            // TODO: use Lerp
            weight -= .05f;
            yield return null;
        }
    }

    IEnumerator DeactivateVolumeToTrigger()
    {
        var weight = 1f;
        while (weight >= 0)
        {
            var newVolumeWeight = 1 - weight;
            foreach (var volume in _allVolumes)
            {
                // If the new weight is greater than the actual weight, but less than the original weight
                if (newVolumeWeight >= volume.weight && _originalVolumeWeights[volume] >= newVolumeWeight) volume.weight = weight;
            }

            yield return new WaitForEndOfFrame();
            if (weight <= _volumeToTrigger.weight) _volumeToTrigger.weight = weight;

            // TODO: use Lerp
            weight -= .05f;
        }
    }
}
