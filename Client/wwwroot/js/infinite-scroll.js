export function initialize({ scrollContainer, lastItem, componentInstance, scrollReverse }) {
    const observerOptions = {
        root: scrollContainer,
        threshold: 0,
        rootMargin: "0px"
    };

    var canScrollToStart = true;
    const observer = new IntersectionObserver(function (entries) {
        if (entries[0].isIntersecting) {
            observer.unobserve(lastItem);
            componentInstance.invokeMethodAsync("OnScrollToEnd")
                .then(function () {
                    if (scrollReverse && canScrollToStart) {
                        scrollContainer.scrollTop = scrollContainer.scrollHeight;
                    }
                    observer.observe(lastItem);
                });
        } else if (!entries[0].isIntersecting) {
            canScrollToStart = false;
        }
    },
        observerOptions);

    observer.observe(lastItem);

    return {
        dispose: () => {
            observer.unobserve(lastItem);
            observer.disconnect();
        }
    };
}