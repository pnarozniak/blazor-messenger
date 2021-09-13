const skeletonLoadingHeight = 144;

const fetchConversations = (dotNetHelper) => {
    return dotNetHelper.invokeMethodAsync("FetchConversationsAsync");
}

const checkSizeAndFetch = (list, dotNetHelper) => {
    if (list.clientHeight >= list.scrollHeight - skeletonLoadingHeight) {
        fetchConversations(dotNetHelper)
            .then((hasFetchedAny) => {
                if (!hasFetchedAny)
                    return undefined;

                return checkSizeAndFetch(list, dotNetHelper);
            });
    }
}

export const runListContainerListeners = (list, dotNetHelper) => {
    checkSizeAndFetch(list, dotNetHelper);

    list.addEventListener("scroll", event => {
        const element = event.target;
        if (element.scrollHeight - element.scrollTop - element.clientHeight < skeletonLoadingHeight)
            fetchConversations(dotNetHelper);
    });

    var timeOutFunctionId;
    window.addEventListener("resize", _ => {
        clearTimeout(timeOutFunctionId);
        timeOutFunctionId = setTimeout(() => {
            checkSizeAndFetch(list, dotNetHelper);
        }, 500);
    });
}